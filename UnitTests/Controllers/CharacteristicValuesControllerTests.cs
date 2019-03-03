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
        public async Task IndexWithoutParametersAsync()
        {
            var expected = new IndexViewModel<CharacteristicValueViewModel>(1, 1, Data.CharacteristicValues.Entities.Count(), from c in Data.CharacteristicValues.Entities select new CharacteristicValueViewModel(c));
            var actual = await Controller.IndexAsync(null, null);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByCategoryAsync()
        {
            var category = Data.Categories.Smartphones;
            
            var categories = new HashSet<Category>();
            var categoryIds = from c in categories select c.Id;
            await GetCategoryParentsAsync(category.Id, categories);
            var characteristics = Data.Characteristics.Entities.Where(c => categoryIds.Contains(c.CategoryId));
            var characteristicValues = Data.CharacteristicValues.Entities.Where(v => characteristics.Contains(v.Characteristic));
            var expected = new IndexViewModel<CharacteristicValueViewModel>(1, 1, characteristicValues.Count(), from c in characteristicValues select new CharacteristicValueViewModel(c));
            var actual = await Controller.IndexAsync(category.Id, null);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByCharacteristicAsync()
        {
            var characteristic = Data.Characteristics.Storage;
            var characteristicValues = Data.CharacteristicValues.Entities.Where(v => v.Characteristic == characteristic);
            var expected = new IndexViewModel<CharacteristicValueViewModel>(1, 1, characteristicValues.Count(), from c in characteristicValues select new CharacteristicValueViewModel(c));
            var actual = await Controller.IndexAsync(null, characteristic.Id);
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

        protected override Task AssertUpdateSuccessAsync(CharacteristicValue beforeUpdate, CharacteristicValueViewModel expected, CharacteristicValueViewModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(beforeUpdate.CharacteristicId, actual.CharacteristicId);
            Assert.Equal(beforeUpdate.Id, actual.Id);
            return Task.CompletedTask;
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
