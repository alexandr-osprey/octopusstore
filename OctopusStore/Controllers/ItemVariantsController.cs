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

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemVariantsController : CRUDController<IItemVariantService, ItemVariant, ItemVariantViewModel>, IItemVariantsController
    {
        public ItemVariantsController(
            IItemVariantService itemVariantService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<ItemVariant, ItemVariantViewModel>> logger)
           : base(itemVariantService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<ItemVariantViewModel> CreateAsync([FromBody]ItemVariantViewModel itemVariantViewModel) => await base.CreateAsync(itemVariantViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<ItemVariantViewModel> ReadAsync(int id) => await base.ReadAsync(new EntitySpecification<ItemVariant>(id));

        [AllowAnonymous]
        [HttpGet("{id:int}/detail")]
        public async Task<ItemVariantDetailViewModel> ReadDetailAsync(int id) => await base.ReadDetailAsync<ItemVariantDetailViewModel>(new ItemVariantDetailSpecification(id));

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemVariantViewModel>> IndexAsync([FromQuery(Name = "itemId")]int itemId) => await IndexByItemAsync(itemId);

        [AllowAnonymous]
        [HttpGet("/api/items/{itemId:int}/itemVariants")]
        public async Task<IndexViewModel<ItemVariantViewModel>> IndexByItemAsync(int itemId) => await base.IndexNotPagedAsync(new ItemVariantByItemSpecification(itemId));

        [HttpPut("{id:int}")]
        public override async Task<ItemVariantViewModel> UpdateAsync([FromBody]ItemVariantViewModel itemVariantViewModel) => await base.UpdateAsync(itemVariantViewModel);

        [HttpDelete("{id}", Name = "ItemVariantDelete")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteSingleAsync(new ItemVariantDetailSpecification(id));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
