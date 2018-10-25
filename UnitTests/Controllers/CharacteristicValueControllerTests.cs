using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class CharacteristicValueControllerTests: ControllerTestBase<CharacteristicValue, CharacteristicValuesController, ICharacteristicValueService>
    {
        private readonly ICategoryService _categoryService;
        private readonly ICharacteristicService _characteristicService;

        public CharacteristicValueControllerTests(ITestOutputHelper output): base(output)
        {
            _categoryService = Resolve<ICategoryService>();
            _characteristicService = Resolve<ICharacteristicService>();
        }

        [Fact]
        public async Task Index()
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Title == "Smartphones");
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var spec = new CharacteristicByCategoryIdsSpecification(categoryIds)
            {
                Take = _maxTake
            };
            var characteristics = await _characteristicService.EnumerateAsync(spec);
            var characteristicIds = from c in characteristics select c.Id;
            var spec2 = new CharacteristicValueByCharacteristicIdsSpecification(characteristicIds)
            {
                Take = _maxTake
            };
            var characteristicValues = await service.EnumerateAsync(spec2);
            var expected = new IndexViewModel<CharacteristicValueViewModel>(1, 1, characteristicValues.Count(), from c in characteristicValues select new  CharacteristicValueViewModel(c));
            var actual = await controller.Index(category.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
    }
}
