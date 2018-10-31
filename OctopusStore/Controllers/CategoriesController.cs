//using System.Threading.Tasks;
//using ApplicationCore.Entities;
//using ApplicationCore.Specifications;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ApplicationCore.ViewModels;
//using ApplicationCore.Interfaces;
//using System.Collections.Generic;
//using System.Linq;
//using ApplicationCore.Interfaces.Services;

//namespace OctopusStore.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/[controller]")]
//    public class CategoriesController: CRUDController<ICategoryService, Category, CategoryViewModel>
//    {
//        public int RootCategoryId = 1;
//        public CategoriesController(
//            ICategoryService service,
//            IScopedParameters scopedParameters,
//            IAppLogger<ICRUDController<Category>> logger)
//           : base(service, scopedParameters, logger)
//        {
//        }

//        // GET: api/<controller>
//        [AllowAnonymous]
//        [HttpGet]
//        public async Task<IndexViewModel<CategoryViewModel>> Index(
//            [FromQuery(Name = "categoryId")]int? categoryId,
//            [FromQuery(Name = "storeId")]int? storeId)
//        {
//            categoryId = categoryId ?? _service.RootCategoryId;
//            var categories = await _service.EnumerateHierarchyAsync(new EntitySpecification<Category>(c => c.Id == categoryId.Value));
//            if (storeId.HasValue)
//            {
//                var storeCategories = await IndexByStoreId(storeId.Value);
//                categories = (from c in categories where storeCategories.Contains(c) select c).OrderBy(c => c.Id);
//            }
//            return GetNotPagedIndexViewModel(categories);
//        }
//        //[AllowAnonymous]
//        //[HttpGet("/api/stores/{id:int}/categories")]
//        //public async Task<CategoryIndexViewModel> IndexByStoreId(int id)
//        //{
//        //    var spec = new Specification<Item>((i => i.StoreId == id), (i => i.Category));
//        //    spec.Description = $"Items with StoreId={id} includes Category";
//        //    return await base.IndexByRelatedNotPagedAsync(_service.EnumerateByItemAsync, spec);
//        //}
//        [HttpGet("{id:int}/checkUpdateAuthorization")]
//        public async Task<ActionResult> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
//        protected async Task<IEnumerable<Category>> IndexByStoreId(int storeId)
//        {
//            var spec = new Specification<Item>((i => i.StoreId == storeId), (i => i.Category))
//            {
//                Description = $"Items with StoreId={storeId} includes Category"
//            };
//            return await _service.EnumerateParentCategoriesAsync(spec);
//        }
//    }
//}
