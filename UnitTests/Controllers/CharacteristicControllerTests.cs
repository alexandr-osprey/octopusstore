using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    public class CharacteristicsControllerTests: ControllerTests<Characteristic, CharacteristicViewModel, ICharacteristicsController, ICharacteristicService>
    {
        private readonly ICategoryService _categoryService;
        public CharacteristicsControllerTests(ITestOutputHelper output) : base(output)
        {
            _categoryService = Resolve<ICategoryService>();
        }

        [Fact]
        public async Task Index()
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Title == "Smartphones");
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var spec = new CharacteristicByCategoryIdsSpecification(categoryIds);
            spec.SetPaging(1, _maxTake);
            var characteristics = await _service.EnumerateAsync(spec);
            var expected = new IndexViewModel<CharacteristicViewModel>(1, 1, characteristics.Count(), from c in characteristics select new CharacteristicViewModel(c));
            var actual = await _controller.Index(category.Id);
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexShoes()
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Title == "Shoes");
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var characteristics = await GetQueryable().Where(c => categoryIds.Contains(c.CategoryId)).ToListAsync();
            var expected = new IndexViewModel<CharacteristicViewModel>(1, 1, characteristics.Count(), from c in characteristics select new CharacteristicViewModel(c));
            var actual = await _controller.Index(category.Id);
            Equal(expected, actual);
        }

        protected override async Task<IEnumerable<Characteristic>> GetCorrectEntitiesToCreateAsync()
        {
            return await Task.FromResult(new List<Characteristic>()
            {
                new Characteristic()
                {
                    CategoryId = 1,
                    Title = "New"
                }
            });
        }

        protected override async Task<IEnumerable<Characteristic>> GetCorrectEntitiesToUpdateAsync()
        {
            var characteristics = await _context.Set<Characteristic>().AsNoTracking().Take(3).ToListAsync();
            characteristics.ForEach(c => c.Title = "updated");
            return characteristics;
        }

        protected override CharacteristicViewModel ToViewModel(Characteristic entity)
        {
            return new CharacteristicViewModel()
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title
            };
        }
    }
}
