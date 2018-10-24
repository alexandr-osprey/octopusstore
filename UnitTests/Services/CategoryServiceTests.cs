using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        {
        }

        [Fact]
        public async Task EnumerateByItemHierarchy()
        {
            int storeId = 1;
            var categories = await context.Items
                .Include(i => i.Category)
                .Where(i => i.StoreId == storeId)
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First())
                .ToListAsync();
            HashSet<Category> expected = new HashSet<Category>();
            foreach (var category in categories)
                await GetCategoryHierarchyAsync(category.Id, expected);
            /// enumerating by items by store

            var actual = await service.EnumerateHierarchyAsync(new Specification<Item>((i => i.StoreId == storeId)));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumeratHierarchy()
        {
            var electronics = await GetQueryable(context)
                .Where(c => c.Title == "Electronics").FirstOrDefaultAsync();

            HashSet<Category> expected = new HashSet<Category>();
           await GetCategoryHierarchyAsync(electronics.Id, expected);
            /// enumerating by items by store

            var actual = await service.EnumerateHierarchyAsync(new EntitySpecification<Category>(electronics.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateSubcategoriesAsync()
        {
            var clothesCategory = await GetQueryable(context)
            .Where(c => c.Title == "Clothes")
            .FirstOrDefaultAsync();
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategorySubcategoriesAsync(clothesCategory.Id, expected);

            var actual = await service.EnumerateSubcategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateParentCategoriesAsync()
        {
            var clothesCategory = await GetQueryable(context)
            .Where(c => c.Title == "Shoes")
            .FirstOrDefaultAsync();
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryParentsAsync(clothesCategory.Id, expected);

            var actual = await service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateSubcategoriesByItemAsync()
        {
            var items = await context.Items
                .Include(i => i.Category)
                .Where(i => i.Title.Contains("Samsung") || i.Title.Contains("Shoes"))
                .ToListAsync();
            var categories = items
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategorySubcategoriesAsync(c.Id, expected));

            var actual = await service.EnumerateSubcategoriesAsync(new EntitySpecification<Item>(i => items.Contains(i)));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateParentCategoriesByItemAsync()
        {
            var items = await context.Items
                .Include(i => i.Category)
                .Where(i => i.Title.Contains("Samsung") || i.Title.Contains("Shoes"))
                .ToListAsync();
            var categories = items
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategoryParentsAsync(c.Id, expected));

            var actual = await service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(c => categories.Contains(c)));
            Equal(expected, actual);
        }
    }
}
