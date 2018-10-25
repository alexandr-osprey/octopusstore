using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using System.Security.Claims;
using System.Security.Authentication;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StoresController: CRUDController<IStoreService, Store, StoreViewModel>
    {
        protected IAuthorizationParameters<Store> _authoriationParameters;

        public StoresController(
            IStoreService storeService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Store> authoriationParameters,
            IAppLogger<ICRUDController<Store>> logger)
           : base(storeService, scopedParameters, logger)
        {
            _authoriationParameters = authoriationParameters;
        }

        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<StoreViewModel>> Index(
            [FromQuery(Name = "page")]int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "updateAuthorizationFilter")]bool? updateAuthorizationFilter,
            [FromQuery(Name = "title")]string title)
        {
            pageSize = pageSize ?? _defaultTake;
            page = page ?? 1;
            if (updateAuthorizationFilter.HasValue && updateAuthorizationFilter.Value)
            {
                if (User.Identity.Name == null)
                    throw new AuthenticationException("Not authenticated");
                _authoriationParameters.ReadAuthorizationRequired = true;
                _authoriationParameters.ReadOperationRequirement = OperationAuthorizationRequirements.Update;
            }
            return await base.IndexAsync(new StoreIndexSpecification(page.Value, pageSize.Value, title));
        }

        // GET api/<controller>/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<StoreViewModel> Get(int id)
        {
            return await base.GetAsync(new EntitySpecification<Store>(id));
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<StoreViewModel> Post([FromBody]StoreViewModel storeViewModel)
        {
            storeViewModel.RegistrationDate = System.DateTime.Now;
            return await base.CreateAsync(storeViewModel);
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<StoreViewModel> Put(int id, [FromBody]StoreViewModel storeViewModel)
        {
            storeViewModel.Id = id;
            return await base.UpdateAsync(storeViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id:int}")]
        public async Task<Response> Delete(int id)
        {
            return await base.DeleteSingleAsync(new EntitySpecification<Store>(id));
        }
        [HttpPost("{storeId:int}/administrators")]
        public async Task<Response> PostStoreAdministrator(int storeId, [FromHeader]string email)
        {
            return await PostDeleteAdministrator(email, storeId, true);
        }
        [HttpDelete("{storeId:int}/administrators")]
        public async Task<Response> DeleteStoreAdministrator(int storeId, [FromHeader]string email)
        {
            return await PostDeleteAdministrator(email, storeId, false);
        }
        [HttpGet("{storeId:int}/administrators")]
        public async Task<IndexViewModel<string>> GetStoreAdministrators(int storeId)
        {
            var store = await _service.ReadSingleAsync(new EntitySpecification<Store>(storeId));
            // this info is confidential, therefore higher requirement needed
            await _service.IdentityService.AuthorizeAsync(User, store, OperationAuthorizationRequirements.Update, true);
            var emails = await _service.IdentityService.EnumerateEmailsWithClaimAsync(new Claim(CustomClaimTypes.StoreAdministrator, storeId.ToString()));
            return IndexViewModel<string>.FromEnumerableNotPaged(emails);
        }
        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);

        protected async Task<Response> PostDeleteAdministrator(string email, int storeId, bool post)
        {
            if (email == null)
                throw new BadRequestException("Administrator email not provided");
            string id = await _service.IdentityService.GetUserId(email);
            var store = await _service.ReadSingleAsync(new EntitySpecification<Store>(storeId));
            await _service.IdentityService.AuthorizeAsync(User, store, OperationAuthorizationRequirements.Update, true);
            var claim = new Claim(CustomClaimTypes.StoreAdministrator, storeId.ToString());
            bool hasClaim = await _service.IdentityService.HasClaimAsync(id, claim);
            if (post && !hasClaim)
                await _service.IdentityService.AddClaim(id, claim);
            else if (!post && hasClaim)
                await _service.IdentityService.RemoveClaim(id, claim);
            string answer = post ? $"{email} now is an administrator": $"{email} is not an administrator anymore";
            return new Response(answer);
        }
    }
}
