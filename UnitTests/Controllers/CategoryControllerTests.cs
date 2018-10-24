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

namespace UnitTests.Controllers
{
    public class CategoryControllerTests : ControllerTestBase<Category, CategoriesController, ICategoryService>
    {
        public CategoryControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexAll()
        {
            var actual = await controller.Index(null, null);
            var rootCategories = await GetQueryable(context)
                .ToListAsync();
            var expected = new CategoryIndexViewModel(1,
                GetPageCount(rootCategories.Count(),
                DefaultSettings.DefaultTake),
                rootCategories.Count(),
                rootCategories);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task IndexClothes()
        {
            var clothes = await GetQueryable(context)
            .FirstOrDefaultAsync(c => c.Title == "Clothes");
            var actual = await controller.Index(clothes.Id, null);
            var spec = new EntitySpecification<Category>(clothes.Id);
            var flatCategories = await service.EnumerateSubcategoriesAsync(spec);
            var expected = new CategoryIndexViewModel(1,
                GetPageCount(flatCategories.Count(), DefaultSettings.DefaultTake),
                flatCategories.Count(),
                flatCategories);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task IndexByStore()
        {
            int storeId = 1;
            var categories = await context.Items
                .Include(i => i.Category)
                .Where(i => i.StoreId == storeId)
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First())
                .ToListAsync();
            HashSet<Category> expectedCategories = new HashSet<Category>();
            foreach (var category in categories)
                await GetCategoryHierarchyAsync(category.Id, expectedCategories);
            var expected = new CategoryIndexViewModel(1, 1, expectedCategories.Count, expectedCategories);
            var actual = await controller.Index(categoryId: null, storeId: storeId);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task IndexWrongCategory()
        {
            var actual = await controller.Index(99696, null);
            var expected = new CategoryIndexViewModel(0, 0, 0, new List<Category>());
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        protected override IQueryable<Category> GetQueryable(DbContext context)
        {
            return base.GetQueryable(context).Include(c => c.Subcategories).ThenInclude(c => c.Subcategories);
        }
    }
}
