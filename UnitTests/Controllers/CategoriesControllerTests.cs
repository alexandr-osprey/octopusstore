using OctopusStore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;

namespace UnitTests.Controllers
{
    public class CategoriesControllerTests : ControllerTests<Category, CategoryViewModel, ICategoriesController, ICategoryService>
    {
        public CategoriesControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexAll()
        {
            var actual = await _controller.IndexAsync(null, null);
            var rootCategories = await GetQueryable()
                .ToListAsync();
            var expected = new IndexViewModel<CategoryViewModel>(1,
                GetPageCount(rootCategories.Count(),
                DefaultSettings.DefaultTake),
                rootCategories.Count(),
                from c in rootCategories select new CategoryViewModel(c));
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexClothes()
        {
            var clothes = await GetQueryable()
            .FirstOrDefaultAsync(c => c.Title == "Clothes");
            var actual = await _controller.IndexAsync(clothes.Id, null);
            var spec = new EntitySpecification<Category>(clothes.Id);
            var flatCategories = await _service.EnumerateHierarchyAsync(spec);
            var expected = new IndexViewModel<CategoryViewModel>(1,
                GetPageCount(flatCategories.Count(), DefaultSettings.DefaultTake),
                flatCategories.Count(),
                (from c in flatCategories select new CategoryViewModel(c)).OrderBy(c => c.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByStore()
        {
            int storeId = 1;
            var categories = await _context.Items
                .Include(i => i.Category)
                .Where(i => i.StoreId == storeId)
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First())
                .ToListAsync();
            HashSet<Category> expectedCategories = new HashSet<Category>();
            foreach (var category in categories)
                await GetCategoryHierarchyAsync(category.Id, expectedCategories);
            var expected = new IndexViewModel<CategoryViewModel>(1, 1, expectedCategories.Count, (from e in expectedCategories select new CategoryViewModel(e)).OrderBy(c => c.Id));
            var actual = await _controller.IndexAsync(categoryId: null, storeId: storeId);
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexWrongCategory()
        {
            var actual = await _controller.IndexAsync(99696, null);
            var expected = new IndexViewModel<CategoryViewModel>(0, 0, 0, new List<CategoryViewModel>());
            Equal(expected, actual);
        }
        protected override IQueryable<Category> GetQueryable()
        {
            return base.GetQueryable().Include(c => c.Subcategories).ThenInclude(c => c.Subcategories);
        }

        protected override async Task<IEnumerable<Category>> GetCorrectEntitiesToCreateAsync()
        {
            return await Task.FromResult(new List<Category>()
            {
                new Category() { ParentCategoryId = 1, Title = "Cat 1", CanHaveItems = false, Description = "desc" },
                new Category() { ParentCategoryId = 2, Title = "Cat 2", CanHaveItems = true, Description = "desc" },
            });
        }

        protected override CategoryViewModel ToViewModel(Category entity)
        {
            return new CategoryViewModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                Title = entity.Title,
                ParentCategoryId = entity.ParentCategoryId
            };
        }

        protected override async Task<IEnumerable<Category>> GetCorrectEntitiesToUpdateAsync()
        {
            var categories = await _context.Set<Category>().Take(3).ToListAsync();
            categories.ForEach(c => c.Title = "updated");
            categories.ForEach(c => c.Description = "updated");
            return categories;
        }
    }
}
