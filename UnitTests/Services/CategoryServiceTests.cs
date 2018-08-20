using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CategoryServiceTests : ServiceTestBase<Category, ICategoryService>
    {
        public CategoryServiceTests(ITestOutputHelper output)
            : base(output)
        { }


        [Fact]
        public async Task ListByStore()
        {
            int storeId = 1;
            var categories = await context.Items
                .Include(i => i.Category)
                .Where(i => i.StoreId == storeId)
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First())
                .ToListAsync();
            List<Category> expected = new List<Category>();
            foreach (var category in categories)
                await GetCategoryHierarchyAsync(category, expected);

            var actual = await service.ListByItemAsync(new Specification<Item>((i => i.StoreId == storeId)));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task GetFlatCategoriesAsync()
        {
            var clothesCategory = await GetQueryable(context)
                .Where(c => c.Title == "Clothes")
                .Include(c => c.Subcategories)
                .FirstOrDefaultAsync();
            ICollection<Category> expected = new List<Category>();
            expected.Add(clothesCategory);
            foreach (var c in clothesCategory.Subcategories)
            {
                expected.Add(c);
            }
            var categorySpec = new CategoryDetailSpecification(clothesCategory.Id);
            var actual = await service.ListHierarchyAsync(categorySpec);
            Assert.Equal(expected, actual);

        }
        private async Task GetParentCategoryIds(int id, List<int> parents)
        {
            var category = await GetQueryable(context).FirstOrDefaultAsync(c => c.Id == id);
            if (category != null && category.ParentCategoryId != 0)
            {
                parents.Add(category.ParentCategoryId);
                await GetParentCategoryIds(category.ParentCategoryId, parents);
            }
        }
    }
}
