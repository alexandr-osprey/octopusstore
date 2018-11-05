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
            int storeId = Data.Stores.Johns.Id;
            var categories = await Context.Items
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

            var actual = await Service.EnumerateHierarchyAsync(new Specification<Item>((i => i.StoreId == storeId)));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumeratHierarchy()
        {
            var electronics = Data.Categories.Electronics;

            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryHierarchyAsync(electronics.Id, expected);
            /// enumerating by items by store

            var actual = await Service.EnumerateHierarchyAsync(new EntitySpecification<Category>(electronics.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateSubcategoriesAsync()
        {
            var clothesCategory = Data.Categories.Clothes;
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategorySubcategoriesAsync(clothesCategory.Id, expected);

            var actual = await Service.EnumerateSubcategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateParentCategoriesAsync()
        {
            var clothesCategory = Data.Categories.Clothes;
            HashSet<Category> expected = new HashSet<Category>();
            await GetCategoryParentsAsync(clothesCategory.Id, expected);

            var actual = await Service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(clothesCategory.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateSubcategoriesByItemAsync()
        {
            var items = Data.Items.Entities;
            var categories = items
                .Select(i => i.CategoryId)
                .GroupBy(i => i)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategorySubcategoriesAsync(c, expected));

            var actual = await Service.EnumerateSubcategoriesAsync(new EntitySpecification<Item>(i => items.Contains(i)));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateParentCategoriesByItemAsync()
        {
            var items = Data.Items.Entities;
            var categories = items
                .Select(i => i.CategoryId)
                .GroupBy(i => i)
                .Select(grp => grp.First()).ToList();

            HashSet<Category> expected = new HashSet<Category>();
            categories.ForEach(async c => await GetCategoryParentsAsync(c, expected));

            var actual = await Service.EnumerateParentCategoriesAsync(new EntitySpecification<Category>(c => categories.Contains(c.Id)));
            expected.SequenceEqual(actual);
            //Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteSingleWithRelatedRelinkAsync()
        {
            var category = Data.Categories.Electronics;
            int idToRelinkTo = Data.Categories.Clothes.Id;
            var items = Data.Items.Entities.Where(i => i.Category == category).ToList();
            var characteristics = Data.Characteristics.Entities.Where(i => i.Category == category).ToList();
            await Service.DeleteSingleWithRelatedRelink(category.Id, idToRelinkTo);
            items.ForEach(i => Assert.Equal(i.CategoryId, idToRelinkTo));
            characteristics.ForEach(c => Assert.Equal(c.CategoryId, idToRelinkTo));
            Assert.False(Context.Set<Category>().Any(b => b == category));
        }

        protected override IEnumerable<Category> GetCorrectNewEntites()
        {
            var parentId = Data.Categories.Root.Id;
            return new List<Category>()
            {
                new Category() { OwnerId = Users.AdminId, Description = "Category 2", Title = "Category 2", ParentCategoryId = parentId },
                new Category() { OwnerId = Users.AdminId, Description = "Category 3", Title = "Category 3", ParentCategoryId = parentId + 1 },
            };
        }

        protected override IEnumerable<Category> GetIncorrectNewEntites()
        {
            var parentId = Data.Categories.Clothes.Id;
            return new List<Category>()
            {
                //new Category() { OwnerId = null, Description = "Category 2", Title = "Category 2", ParentCategoryId = 1 },
                new Category() { OwnerId = Users.AdminId, Description = " ", Title = "Category 3", ParentCategoryId = parentId },
                new Category() { OwnerId = Users.AdminId, Description = "Category 3", Title = "Category 3", ParentCategoryId = 99 },
                new Category() { OwnerId = Users.AdminId, Description = "Category 3", Title = "", ParentCategoryId = parentId },
            };
        }

        protected override IEnumerable<Category> GetCorrectEntitesForUpdate()
        {
            Data.Categories.Shoes.Title = "Tit1";
            Data.Categories.Shoes.Description = "Desc1";
            return new List<Category>() { Data.Categories.Shoes };
        }

        protected override IEnumerable<Category> GetIncorrectEntitesForUpdate()
        {
            Data.Categories.Electronics.Description = " ";
            Data.Categories.Jackets.Title = "";
            return new List<Category>()
            {
                Data.Categories.Electronics,
                Data.Categories.Jackets
            };
        }

        protected override Specification<Category> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Category>(c => c.Title == "Shoes");
        }
    }
}
