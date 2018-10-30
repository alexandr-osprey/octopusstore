using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CategoryServiceTests : ServiceTests<Category, ICategoryService>
    {
        public CategoryServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        [Fact]
        public async Task EnumerateByItemHierarchy()
        {
            int storeId = 1;
            var categories = await _context.Items
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

            var actual = await _service.EnumerateHierarchyAsync(new Specification<Item>((i => i.StoreId == storeId)));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumeratHierarchy()
        {
            var electronics = await GetQueryable()
                .Where(c => c.Title == "Electronics").FirstOrDefaultAsync();

            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryHierarchyAsync(electronics.Id, expected);
            /// enumerating by items by store

            var actual = await _service.EnumerateHierarchyAsync(new EntitySpecification<Category>(electronics.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateSubcategoriesAsync()
        {
            var clothesCategory = await GetQueryable()
            .Where(c => c.Title == "Clothes")
            .FirstOrDefaultAsync();
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategorySubcategoriesAsync(clothesCategory.Id, expected);

            var actual = await _service.EnumerateSubcategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateParentCategoriesAsync()
        {
            var clothesCategory = await GetQueryable()
            .Where(c => c.Title == "Shoes")
            .FirstOrDefaultAsync();
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryParentsAsync(clothesCategory.Id, expected);

            var actual = await _service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateSubcategoriesByItemAsync()
        {
            var items = await _context.Items
                .Include(i => i.Category)
                .Where(i => i.Title.Contains("Samsung") || i.Title.Contains("Shoes"))
                .ToListAsync();
            var categories = items
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategorySubcategoriesAsync(c.Id, expected));

            var actual = await _service.EnumerateSubcategoriesAsync(new EntitySpecification<Item>(i => items.Contains(i)));
            Equal(expected, actual);
        }
        [Fact]
        public async Task EnumerateParentCategoriesByItemAsync()
        {
            var items = await _context.Items
                .Include(i => i.Category)
                .Where(i => i.Title.Contains("Samsung") || i.Title.Contains("Shoes"))
                .ToListAsync();
            var categories = items
                .Select(i => i.Category)
                .GroupBy(i => i.Id)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategoryParentsAsync(c.Id, expected));

            var actual = await _service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(c => categories.Contains(c)));
            Equal(expected, actual);
        }

        protected override async Task<IEnumerable<Category>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<Category>()
            {
                new Category() { OwnerId = adminId, Description = "Category 2", Title = "Category 2", ParentCategoryId = 1 },
                new Category() { OwnerId = adminId, Description = "Category 3", Title = "Category 3", ParentCategoryId = 2 },
            });
        }

        protected override async Task<IEnumerable<Category>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<Category>()
            {
                //new Category() { OwnerId = null, Description = "Category 2", Title = "Category 2", ParentCategoryId = 1 },
                new Category() { OwnerId = adminId, Description = " ", Title = "Category 3", ParentCategoryId = 2 },
                new Category() { OwnerId = adminId, Description = "Category 3", Title = "Category 3", ParentCategoryId = 99 },
                new Category() { OwnerId = adminId, Description = "Category 3", Title = "", ParentCategoryId = 2 },
            });
        }

        protected override async Task<IEnumerable<Category>> GetCorrectEntitesForUpdateAsync()
        {
            var shoes = await GetQueryable().FirstOrDefaultAsync(c => c.Title == "Shoes");
            shoes.Title = "Tit1";
            shoes.Description = "Desc1";
            return new List<Category>()
            {
                shoes
            };
        }

        protected override async Task<IEnumerable<Category>> GetIncorrectEntitesForUpdateAsync()
        {
            return await Task.FromResult(new List<Category>()
            {
                new Category() { Id = 2, Description = " ", Title = "Category 2" },
                new Category() { Id = 3, Description = "Category 3", Title = "Category 3", ParentCategoryId = 99 },
                new Category() { Id = 2, Description = "c", Title = "" },
            });
        }

        protected override Specification<Category> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Category>(c => c.Title == "Shoes");
        }
    }
}
