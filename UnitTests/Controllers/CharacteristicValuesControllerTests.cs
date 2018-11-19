using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    public class CharacteristicValuesControllerTests : ControllerTests<CharacteristicValue, CharacteristicValueViewModel, ICharacteristicValuesController, ICharacteristicValueService>
    {
        private readonly ICategoryService _categoryService;
        private readonly ICharacteristicService _characteristicService;

        public CharacteristicValuesControllerTests(ITestOutputHelper output) : base(output)
        {
            _categoryService = Resolve<ICategoryService>();
            _characteristicService = Resolve<ICharacteristicService>();
        }

        [Fact]
        public async Task IndexAsync()
        {
            var category = Data.Categories.Smartphones;
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var spec = new CharacteristicByCategoryIdsSpecification(categoryIds);
            var characteristics = await _characteristicService.EnumerateAsync(spec);
            var characteristicIds = from c in characteristics select c.Id;
            var spec2 = new CharacteristicValueByCharacteristicIdsSpecification(characteristicIds);
            var characteristicValues = await Service.EnumerateAsync(spec2);
            var expected = new IndexViewModel<CharacteristicValueViewModel>(1, 1, characteristicValues.Count(), from c in characteristicValues select new CharacteristicValueViewModel(c));
            var actual = await Controller.IndexAsync(category.Id, null);
            Equal(expected, actual);
        }

        protected override IEnumerable<CharacteristicValue> GetCorrectEntitiesToCreate()
        {
            return new List<CharacteristicValue>()
            {
                new CharacteristicValue()
                {
                    Title = "new",
                    CharacteristicId = Data.Characteristics.Colour.Id
                }
            };
        }

        protected override void AssertUpdateSuccess(CharacteristicValue beforeUpdate, CharacteristicValueViewModel expected, CharacteristicValueViewModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(beforeUpdate.CharacteristicId, actual.CharacteristicId);
            Assert.Equal(beforeUpdate.Id, actual.Id);
        }

        protected override IEnumerable<CharacteristicValueViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<CharacteristicValueViewModel>()
            {
                new CharacteristicValueViewModel()
                {
                    Id = Data.CharacteristicValues.GB64.Id,
                    CharacteristicId = 999,
                    Title = "UPDATED"
                }
            };
        }

        protected override CharacteristicValueViewModel ToViewModel(CharacteristicValue entity)
        {
            return new CharacteristicValueViewModel()
            {
                Id = entity.Id,
                CharacteristicId = entity.CharacteristicId,
                Title = entity.Title
            };
        }
    }
}
