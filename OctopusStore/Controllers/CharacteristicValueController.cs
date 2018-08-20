using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CharacteristicValuesController
        : ReadController<
            ICharacteristicValueService, 
            CharacteristicValue,
            CharacteristicValueViewModel,
            CharacteristicValueDetailViewModel,
            CharacteristicValueIndexViewModel>
    {

        private readonly ICharacteristicService _characteristicService;
        private readonly ICategoryService _categoryService;

        public CharacteristicValuesController(
            ICharacteristicValueService service, 
            ICategoryService categoryService,
            ICharacteristicService characteristicService,
            IAppLogger<IEntityController<CharacteristicValue>> logger)
            : base(service, logger)
        {
            _characteristicService = characteristicService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<CharacteristicValueIndexViewModel> Index([FromQuery(Name = "categoryId")]int categoryId)
        {
            return await CategoryCharacteristicValuesIndex(categoryId);
        }
        [HttpGet("/api/categories/{categoryId:int}/characteristicValues")]
        public async Task<CharacteristicValueIndexViewModel> CategoryCharacteristicValuesIndex(int categoryId)
        {
            var categoryIds = from category 
                              in await _categoryService.ListHierarchyAsync(new Specification<Category>(categoryId))
                              select category.Id;
            var characteristicIds = from characteristic 
                                    in await _characteristicService.ListAsync(new CharacteristicByCategoryIdsSpecification(categoryIds))
                                    select characteristic.Id;
            var spec = new CharacteristicValueByCharacteristicIdsSpecification(characteristicIds);
            return await base.IndexNotPagedAsync(spec);
        }
    }
}
