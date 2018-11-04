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
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StoresController : CRUDController<IStoreService, Store, StoreViewModel>, IStoresController
    {
        protected IAuthorizationParameters<Store> AuthorziationParameters { get; }

        public StoresController(
            IStoreService storeService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Store> authoriationParameters,
            IAppLogger<IController<Store, StoreViewModel>> logger)
           : base(storeService, scopedParameters, logger)
        {
            AuthorziationParameters = authoriationParameters;
        }

        [HttpPost]
        public override async Task<StoreViewModel> CreateAsync([FromBody]StoreViewModel storeViewModel) => await base.CreateAsync(storeViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [HttpGet("{id:int}/detail")]
        public override async Task<StoreViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<StoreViewModel>> IndexAsync(
            [FromQuery(Name = "page")]int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "updateAuthorizationFilter")]bool? updateAuthorizationFilter,
            [FromQuery(Name = "title")]string title)
        {
            pageSize = pageSize ?? DefaultTake;
            page = page ?? 1;
            if (updateAuthorizationFilter.HasValue && updateAuthorizationFilter.Value)
            {
                if (User.Identity.Name == null)
                    throw new AuthenticationException("Not authenticated");
                AuthorziationParameters.ReadAuthorizationRequired = true;
                AuthorziationParameters.ReadOperationRequirement = OperationAuthorizationRequirements.Update;
            }
            return await base.IndexAsync(new StoreIndexSpecification(page.Value, pageSize.Value, title));
        }

        [HttpPut("{id:int}")]
        public override async Task<StoreViewModel> UpdateAsync([FromBody]StoreViewModel storeViewModel) => await base.UpdateAsync(storeViewModel);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteAsync(id);

        [HttpPost("{storeId:int}/administrators")]
        public async Task<Response> CreateStoreAdministratorAsync(int storeId, [FromHeader]string email) => await CreateDeleteAdministrator(email, storeId, true);

        [HttpGet("{storeId:int}/administrators")]
        public async Task<IndexViewModel<string>> GetStoreAdministratorsAsync(int storeId)
        {
            var store = await Service.ReadSingleAsync(new EntitySpecification<Store>(storeId));
            // this info is confidential, therefore higher requirement needed
            await Service.IdentityService.AuthorizeAsync(User, store, OperationAuthorizationRequirements.Update, true);
            var emails = await Service.IdentityService.EnumerateEmailsWithClaimAsync(new Claim(CustomClaimTypes.StoreAdministrator, storeId.ToString()));
            return IndexViewModel<string>.FromEnumerableNotPaged(emails);
        }

        [HttpDelete("{storeId:int}/administrators")]
        public async Task<Response> DeleteStoreAdministratorAsync(int storeId, [FromHeader]string email) => await CreateDeleteAdministrator(email, storeId, false);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);

        protected async Task<Response> CreateDeleteAdministrator(string email, int storeId, bool create)
        {
            if (email == null)
                throw new BadRequestException("Administrator email not provided");
            string id = await Service.IdentityService.GetUserId(email);
            var store = await Service.ReadSingleAsync(new EntitySpecification<Store>(storeId));
            await Service.IdentityService.AuthorizeAsync(User, store, OperationAuthorizationRequirements.Update, true);
            var claim = new Claim(CustomClaimTypes.StoreAdministrator, storeId.ToString());
            bool hasClaim = await Service.IdentityService.HasClaimAsync(id, claim);
            if (create && !hasClaim)
                await Service.IdentityService.AddClaim(id, claim);
            else if (!create && hasClaim)
                await Service.IdentityService.RemoveClaim(id, claim);
            string answer = create ? $"{email} now is an administrator" : $"{email} is not an administrator anymore";
            return new Response(answer);
        }
    }
}
