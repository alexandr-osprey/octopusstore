using OctopusStore;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
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
        public async Task IndexAllAsync()
        {
            var actual = await Controller.IndexAsync(null, null);
            var allCategories = Data.Categories.Entities;
            var expected = new IndexViewModel<CategoryViewModel>(1,
                GetPageCount(allCategories.Count(),
                DefaultSettings.DefaultTake),
                allCategories.Count(),
                from c in allCategories select new CategoryViewModel(c));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexClothesAsync()
        {
            var clothes = Data.Categories.Clothes;
            var actual = await Controller.IndexAsync(clothes.Id, null);
            var spec = new EntitySpecification<Category>(clothes.Id);
            var flatCategories = await Service.EnumerateHierarchyAsync(spec);
            var expected = new IndexViewModel<CategoryViewModel>(1,
                GetPageCount(flatCategories.Count(), DefaultSettings.DefaultTake),
                flatCategories.Count(),
                (from c in flatCategories select new CategoryViewModel(c)).OrderBy(c => c.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByStoreAsync()
        {
            int storeId = Data.Stores.Johns.Id;
            var categories = await Context.Items
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
            var actual = await Controller.IndexAsync(categoryId: null, storeId: storeId);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWrongCategoryAsync()
        {
            var actual = await Controller.IndexAsync(99696, null);
            var expected = new IndexViewModel<CategoryViewModel>(0, 0, 0, new List<CategoryViewModel>());
            Equal(expected, actual);
        }
        protected override IQueryable<Category> GetQueryable()
        {
            return base.GetQueryable().Include(c => c.Subcategories).ThenInclude(c => c.Subcategories);
        }

        protected override IEnumerable<Category> GetCorrectEntitiesToCreate()
        {
            return new List<Category>()
            {
                new Category() { ParentCategoryId = Data.Categories.Root.Id, Title = "Cat 1", CanHaveItems = false, Description = "desc" },
                new Category() { ParentCategoryId = Data.Categories.Clothes.Id, Title = "Cat 2", CanHaveItems = true, Description = "desc" },
            };
        }

        protected override CategoryViewModel ToViewModel(Category entity)
        {
            return new CategoryViewModel()
            {
                Id = entity.Id,
                IsRoot = entity.IsRoot,
                Description = entity.Description,
                Title = entity.Title,
                ParentCategoryId = entity.ParentCategoryId
            };
        }

        protected override IEnumerable<CategoryViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<CategoryViewModel>()
            {
                new CategoryViewModel()
                {
                    Id = Data.Categories.Shoes.Id,
                    Description = "UPDATED",
                    IsRoot = false,
                    ParentCategoryId = Data.Categories.Root.Id,
                    Title ="UPDATED"
                }
            };
        }
    }
}
