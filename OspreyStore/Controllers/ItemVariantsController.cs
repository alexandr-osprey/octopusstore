using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Identity;

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemVariantsController : CRUDController<IItemVariantService, ItemVariant, ItemVariantViewModel>, IItemVariantsController
    {
        public ItemVariantsController(
            IItemVariantService itemVariantService,
            IActivatorService activatorService,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<ItemVariant, ItemVariantViewModel>> logger)
           : base(itemVariantService, activatorService, identityService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<ItemVariantViewModel> CreateAsync([FromBody]ItemVariantViewModel itemVariantViewModel) => await base.CreateAsync(itemVariantViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<ItemVariantViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet("{id:int}/detail")]
        public async Task<ItemVariantDetailViewModel> ReadDetailAsync(int id) => await base.ReadAsync<ItemVariantDetailViewModel>(new ItemVariantDetailSpecification(id));

        [AllowAnonymous]
        [HttpGet("{id:int}/thumbnail")]
        public async Task<ItemVariantThumbnailViewModel> ReadThumbnailAsync(int id) => await base.ReadAsync<ItemVariantThumbnailViewModel>(new ItemVariantThumbnailSpecification(id));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemVariantViewModel>> IndexAsync([FromQuery(Name = "itemId")]int itemId) => await IndexByItemAsync(itemId);

        [AllowAnonymous]
        [HttpGet("/api/items/{itemId:int}/itemVariants")]
        public async Task<IndexViewModel<ItemVariantViewModel>> IndexByItemAsync(int itemId) => await base.IndexNotPagedAsync(new ItemVariantByItemSpecification(itemId));

        [HttpPut]
        public override async Task<ItemVariantViewModel> UpdateAsync([FromBody]ItemVariantViewModel itemVariantViewModel) => await base.UpdateAsync(itemVariantViewModel);

        [HttpDelete("{id}", Name = "ItemVariantDelete")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteSingleAsync(new ItemVariantDetailSpecification(id));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
