using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class CharacteristicValueControllerTests : ControllerTestBase<CharacteristicValue, CharacteristicValueController, ICharacteristicValueService>
    {
        private readonly ICategoryService _categoryService;
        private readonly ICharacteristicService _characteristicService;

        public CharacteristicValueControllerTests(ITestOutputHelper output) : base(output)
        {
            _categoryService = Resolve<ICategoryService>();
            _characteristicService = Resolve<ICharacteristicService>();
        }

        [Fact]
        public async Task Index()
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Title == "Smartphones");
            var categories = await _categoryService.ListHierarchyAsync(new Specification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var spec = new CharacteristicByCategoryIdsSpecification(categoryIds);
            spec.Take = _maxTake;
            var characteristics = await _characteristicService.ListAsync(spec);
            var characteristicIds = from c in characteristics select c.Id;
            var spec2 = new CharacteristicValueByCharacteristicIdsSpecification(characteristicIds);
            spec2.Take = _maxTake;
            var characteristicValues = await service.ListAsync(spec2);
            var expected = new CharacteristicValueIndexViewModel(1, 1, characteristicValues.Count(), characteristicValues);
            var actual = await controller.Index(category.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
    }
}
