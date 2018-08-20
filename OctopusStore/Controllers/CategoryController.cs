using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoriesController
        : ReadController<
            ICategoryService,
            Category,
            CategoryViewModel,
            CategoryDetailViewModel,
            CategoryIndexViewModel>
    {
        public CategoriesController(ICategoryService categoryService, IAppLogger<IEntityController<Category>> logger)
            : base(categoryService, logger)
        { }

        // GET: api/<controller>
        [HttpGet]
        public async Task<CategoryIndexViewModel> Index(
            [FromQuery(Name = "categoryId")]int? categoryId,
            [FromQuery(Name = "storeId")]int? storeId)
        {
            if (storeId.HasValue)
            {
                return await IndexByStoreId(storeId.Value);
            }
            else
            {
                categoryId = categoryId ?? _serivce.RootCategoryId;
                var spec = new Specification<Category>((c => c.Id == categoryId.Value), (c => c.Subcategories));
                spec.Description += " includes Subcatetories";
                return await base.IndexByFunctionNotPagedAsync(spec, _serivce.ListSubcategoriesAsync);
            }
        }
        [HttpGet("/api/stores/{id:int}/categories")]
        public async Task<CategoryIndexViewModel> IndexByStoreId(int id)
        {
            var spec = new Specification<Item>((i => i.StoreId == id), (i => i.Category));
            spec.Description = $"Items with StoreId={id} includes Category";
            return await base.IndexByRelatedNotPagedAsync(_serivce.ListByItemAsync, spec);
        }
    }
}
