using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/characteristics")]
    public class CharacteristicController
        : ReadController<
            ICharacteristicService, 
            Characteristic,
            CharacteristicViewModel,
            CharacteristicDetailViewModel,
            CharacteristicIndexViewModel>
    {
        private readonly ICategoryService _categoryService;

        public CharacteristicController(
            ICharacteristicService service, 
            ICategoryService categoryService,
            ICharacteristicService characteristicService,
            IAppLogger<IEntityController<Characteristic>> logger)
            : base(service, logger)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<CharacteristicIndexViewModel> Index([FromQuery(Name = "categoryId")]int categoryId)
        {
            return await CategoryCharacteristicsIndex(categoryId);
        }
        [HttpGet("/api/categories/{categoryId:int}/characteristics")]
        public async Task<CharacteristicIndexViewModel> CategoryCharacteristicsIndex(int categoryId)
        {
            var categories = await _categoryService.ListHierarchyAsync(new Specification<Category>(categoryId));
            var categoryIds = from category 
                              in categories
                              select category.Id;
            return await base.IndexNotPagedAsync(new CharacteristicByCategoryIdsSpecification(categoryIds));
        }
    }
}
