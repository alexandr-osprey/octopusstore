using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data.SampleData;
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
            int storeId = _data.Stores.Johns.Id;
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
            var electronics = _data.Categories.Electronics;

            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryHierarchyAsync(electronics.Id, expected);
            /// enumerating by items by store

            var actual = await _service.EnumerateHierarchyAsync(new EntitySpecification<Category>(electronics.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateSubcategoriesAsync()
        {
            var clothesCategory = _data.Categories.WomensClothing;
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategorySubcategoriesAsync(clothesCategory.Id, expected);

            var actual = await _service.EnumerateSubcategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateParentCategoriesAsync()
        {
            var clothesCategory = _data.Categories.WomensClothing;
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryParentsAsync(clothesCategory.Id, expected);

            var actual = await _service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateSubcategoriesByItemAsync()
        {
            var items = _data.Items.Entities;
            var categories = items
                .Select(i => i.CategoryId)
                .GroupBy(i => i)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategorySubcategoriesAsync(c, expected));

            var actual = await _service.EnumerateSubcategoriesAsync(new EntitySpecification<Item>(i => items.Contains(i)));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateParentCategoriesByItemAsync()
        {
            var items = _data.Items.Entities;
            var categories = items
                .Select(i => i.CategoryId)
                .GroupBy(i => i)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategoryParentsAsync(c, expected));

            var actual = await _service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(c => categories.Contains(c.Id)));
            expected.SequenceEqual(actual);
            //Equal(expected, actual);
        }

        protected override IEnumerable<Category> GetCorrectNewEntites()
        {
            var parentId = _data.Categories.Root.Id;
            return new List<Category>()
            {
                new Category() { OwnerId = Users.AdminId, Title = "Category 2", ParentCategoryId = parentId },
                new Category() { OwnerId = Users.AdminId, Title = "Category 3", ParentCategoryId = parentId + 1 },
            };
        }

        protected override IEnumerable<Category> GetIncorrectNewEntites()
        {
            var parentId = _data.Categories.WomensClothing.Id;
            return new List<Category>()
            {
                //new Category() { OwnerId = null, Description = "Category 2", Title = "Category 2", ParentCategoryId = 1 },
                new Category() { OwnerId = Users.AdminId, Title = "Category 3", ParentCategoryId = 99 },
                new Category() { OwnerId = Users.AdminId, Title = "", ParentCategoryId = parentId },
            };
        }

        protected override IEnumerable<Category> GetCorrectEntitesForUpdate()
        {
            _data.Categories.WomensFootwear.Title = "Tit1";
            return new List<Category>() { _data.Categories.WomensFootwear };
        }

        protected override IEnumerable<Category> GetIncorrectEntitesForUpdate()
        {
            _data.Categories.WomensDresses.Title = "";
            _data.Categories.Smartphones.ParentCategoryId = _data.Categories.WomensClothing.Id;
            _data.Categories.WomensClothing.IsRoot = !_data.Categories.WomensClothing.IsRoot;
            return new List<Category>()
            {
                _data.Categories.WomensDresses,
                _data.Categories.Smartphones,
                _data.Categories.WomensClothing
            };
        }

        protected override Specification<Category> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Category>(c => c.Title == "Shoes");
        }
    }
}
