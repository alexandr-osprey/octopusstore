//using System.Threading.Tasks;
//using ApplicationCore.Entities;
//using ApplicationCore.Specifications;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ApplicationCore.ViewModels;
//using ApplicationCore.Interfaces;
//using ApplicationCore.Interfaces.Services;

//namespace OctopusStore.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/[controller]")]
//    public class ItemsController: CRUDController<IItemService, Item, ItemViewModel>
//    {
//        public ItemsController(
//            IItemService service,
//            IScopedParameters scopedParameters,
//            IAuthorizationService authorizationService,
//            IAppLogger<ICRUDController<Item>> logger)
//           : base(service, scopedParameters, logger)
//        {
//        }

//        //
//        // POST: api/Items
//        [HttpPost]
//        public async Task<ItemViewModel> Post([FromBody]ItemViewModel itemViewModel)
//        {
//            return await base.CreateAsync(itemViewModel);
//        }

//        // GET: api/Items
//        //[AllowAnonymous]
//        [HttpGet("thumbnails/")]
//        public async Task<IndexViewModel<ItemThumbnailViewModel>> IndexThumbnails(
//            [FromQuery(Name = "page")] int? page,
//            [FromQuery(Name = "pageSize")]int? pageSize,
//            [FromQuery(Name = "title")]string title,
//            [FromQuery(Name = "categoryId")]int? categoryId,
//            [FromQuery(Name = "storeId")]int? storeId,
//            [FromQuery(Name = "brandId")]int? brandId)
//        {
//            page = page ?? 1;
//            pageSize = pageSize ?? _defaultTake;
//            var spec = await _service.GetIndexSpecificationByParameters(page.Value, pageSize.Value, title, categoryId, storeId, brandId);
//            return await base.IndexAsync<ItemThumbnailViewModel>(new ItemThumbnailIndexSpecification(spec));
//        }

//       //[AllowAnonymous]
//        [HttpGet]
//        public async Task<IndexViewModel<ItemViewModel>> Index(
//            [FromQuery(Name = "page")] int? page,
//            [FromQuery(Name = "pageSize")]int? pageSize,
//            [FromQuery(Name = "title")]string title,
//            [FromQuery(Name = "categoryId")]int? categoryId,
//            [FromQuery(Name = "storeId")]int? storeId,
//            [FromQuery(Name = "brandId")]int? brandId)
//        {
//            page = page ?? 1;
//            pageSize = pageSize ?? _defaultTake;
//            return await base.IndexAsync(await _service.GetIndexSpecificationByParameters(page.Value, pageSize.Value, title, categoryId, storeId, brandId));
//        }
//        //[AllowAnonymous]
//        [HttpGet("{id:int}")]
//        public async Task<ItemViewModel> Get(int id)
//        {
//            return await base.GetAsync(new EntitySpecification<Item>(id));
//        }
//        //[AllowAnonymous]
//        [HttpGet("{id:int}/details")]
//        public async Task<ItemDetailViewModel> GetDetail(int id)
//        {
//            return await base.GetDetailAsync<ItemDetailViewModel>(new ItemDetailSpecification(id));
//        }

//        [HttpGet("{id:int}/checkUpdateAuthorization")]
//        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
//        // PUT: api/Items/5
//        [HttpPut("{id:int}")]
//        public async Task<ItemViewModel> Put(int id, [FromBody]ItemViewModel itemViewModel)
//        {
//            itemViewModel.Id = id;
//            return await base.UpdateAsync(itemViewModel);
//        }
//        // DELETE: api/ApiWithActions/5
//        [Authorize]
//        [HttpDelete("{id:int}")]
//        public async Task<Response> Delete(int id)
//        {
//            return await base.DeleteSingleAsync(new ItemDetailSpecification(id));
//        }
//    }
//}
