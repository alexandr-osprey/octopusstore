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

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StoresController : CRUDController<IStoreService, Store, StoreViewModel>, IStoresController
    {
        protected IAuthorizationParameters<Store> AuthorziationParameters { get; }

        public StoresController(
            IStoreService storeService,
            IActivatorService activatorService,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Store> authoriationParameters,
            IAppLogger<IController<Store, StoreViewModel>> logger)
           : base(storeService, activatorService, identityService, scopedParameters, logger)
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
            if (updateAuthorizationFilter.HasValue && updateAuthorizationFilter.Value)
            {
                if (User.Identity.Name == null)
                    throw new AuthenticationException("Not authenticated");
                AuthorziationParameters.ReadAuthorizationRequired = true;
                AuthorziationParameters.ReadOperationRequirement = OperationAuthorizationRequirements.Update;
            }
            return await base.IndexAsync(new StoreIndexSpecification(page ?? 1, pageSize ?? _defaultTake, title));
        }

        [HttpPut]
        public override async Task<StoreViewModel> UpdateAsync([FromBody]StoreViewModel storeViewModel) => await base.UpdateAsync(storeViewModel);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteAsync(id);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
