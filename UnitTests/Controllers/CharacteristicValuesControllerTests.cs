using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
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
        public async Task Index()
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Title == "Smartphones");
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var spec = new CharacteristicByCategoryIdsSpecification(categoryIds);
            var characteristics = await _characteristicService.EnumerateAsync(spec);
            var characteristicIds = from c in characteristics select c.Id;
            var spec2 = new CharacteristicValueByCharacteristicIdsSpecification(characteristicIds);
            var characteristicValues = await _service.EnumerateAsync(spec2);
            var expected = new IndexViewModel<CharacteristicValueViewModel>(1, 1, characteristicValues.Count(), from c in characteristicValues select new CharacteristicValueViewModel(c));
            var actual = await _controller.Index(category.Id);
            Equal(expected, actual);
        }

        protected override async Task<IEnumerable<CharacteristicValue>> GetCorrectEntitiesToCreateAsync()
        {
            return await Task.FromResult(new List<CharacteristicValue>()
            {
                new CharacteristicValue()
                {
                    Title = "new",
                    CharacteristicId = 2
                }
            });
        }

        protected override async Task<IEnumerable<CharacteristicValue>> GetCorrectEntitiesToUpdateAsync()
        {
            var entities = await _context.Set<CharacteristicValue>().AsNoTracking().Take(3).ToListAsync();
            entities.ForEach(e => e.Title = "updated");
            return entities;
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
