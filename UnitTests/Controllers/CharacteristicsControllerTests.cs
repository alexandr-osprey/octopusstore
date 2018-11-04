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
        protected readonly ICategoryService _categoryService;

        public CharacteristicsControllerTests(ITestOutputHelper output) : base(output)
        {
            _categoryService = Resolve<ICategoryService>();
        }

        [Fact]
        public async Task IndexAsync()
        {
            var category = Data.Categories.Smartphones;
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var spec = new CharacteristicByCategoryIdsSpecification(categoryIds);
            spec.SetPaging(1, MaxTake);
            var characteristics = await Service.EnumerateAsync(spec);
            var expected = new IndexViewModel<CharacteristicViewModel>(1, 1, characteristics.Count(), from c in characteristics select new CharacteristicViewModel(c));
            var actual = await Controller.IndexAsync(category.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexShoes()
        {
            var category = Data.Categories.Shoes;
            var categories = await _categoryService.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(category.Id));
            var categoryIds = from c in categories select c.Id;
            var characteristics = await GetQueryable().Where(c => categoryIds.Contains(c.CategoryId)).ToListAsync();
            var expected = new IndexViewModel<CharacteristicViewModel>(1, 1, characteristics.Count(), from c in characteristics select new CharacteristicViewModel(c));
            var actual = await Controller.IndexAsync(category.Id);
            Equal(expected, actual);
        }

        protected override void AssertUpdateSuccess(Characteristic beforeUpdate, CharacteristicViewModel expected, CharacteristicViewModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(beforeUpdate.CategoryId, actual.CategoryId);
            Assert.Equal(beforeUpdate.Id, actual.Id);
        }

        protected override IEnumerable<Characteristic> GetCorrectEntitiesToCreate()
        {
            return new List<Characteristic>()
            {
                new Characteristic()
                {
                    CategoryId = Data.Categories.Root.Id,
                    Title = "New"
                }
            };
        }

        protected override IEnumerable<CharacteristicViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<CharacteristicViewModel>()
            {
                new CharacteristicViewModel()
                {
                    Id = Data.Characteristics.Storage.Id,
                    Title = "UPDATED"
                }
            };
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
