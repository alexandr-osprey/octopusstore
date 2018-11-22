using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    public class ItemControllerTests : ControllerTests<Item, ItemViewModel, IItemsController, IItemService>
    {

        public ItemControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexWithoutFiltersAsync()
        {
            int page = 2;
            int pageSize = 3;

            var actual = (await Controller.IndexAsync(page, pageSize, null, null, null, null, null, null, null));
            var items = Data.Items.Entities
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = Data.Items.Entities.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithCharacteristicValuesFiltersAsync()
        {
            int page = 1;
            int pageSize = 5;
            HashSet<CharacteristicValue> characteristicValues = new HashSet<CharacteristicValue>
            {
                Data.CharacteristicValues.GB32,
                Data.CharacteristicValues.HD
            };
            string filter = string.Join(';', (from c in characteristicValues select c.Id.ToString()));
            var actual = (await Controller.IndexAsync(page, pageSize, null, null, null, null, null, filter, null));
            var items = Data.Items.Entities.Where(i => characteristicValues.IsSubsetOf(from p in i.ItemVariants.SelectMany(v => v.ItemProperties) select p.CharacteristicValue));
            int totalCount = items.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexThumbnailsWithCharacteristicValuesFiltersAsync()
        {
            int page = 1;
            int pageSize = 5;
            HashSet<CharacteristicValue> characteristicValues = new HashSet<CharacteristicValue>
            {
                Data.CharacteristicValues.GB32,
                Data.CharacteristicValues.HD
            };
            string filter = string.Join(';', (from c in characteristicValues select c.Id.ToString()));
            var actual = (await Controller.IndexThumbnailsAsync(page, pageSize, null, null, null, null, null, filter, null));
            var items = Data.Items.Entities.Where(i => characteristicValues.IsSubsetOf(from p in i.ItemVariants.SelectMany(v => v.ItemProperties) select p.CharacteristicValue));
            int totalCount = items.Count();
            var expected = new IndexViewModel<ItemThumbnailViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemThumbnailViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithWrongCharacteristicValuesFiltersAsync()
        {
            int page = 1;
            int pageSize = 5;

            var emptyActual = (await Controller.IndexAsync(page, pageSize, null, null, null, null, null, "9999;8888", null));
            Assert.Empty(emptyActual.Entities);

            HashSet<CharacteristicValue> wrongCharacteristicValues = new HashSet<CharacteristicValue>
            {
                Data.CharacteristicValues.XXL,
                Data.CharacteristicValues.HD
            };
            string filter = string.Join(';', (from c in wrongCharacteristicValues select c.Id.ToString()));

            var emptyActual2 = (await Controller.IndexAsync(page, pageSize, null, null, null, null, null, filter, null));
            Assert.Empty(emptyActual2.Entities);
        }

        [Fact]
        public async Task IndexWithoutFiltersWithoutPageSizeAsync()
        {
            int page = 2;
            int take = 50;

            var actual = (await Controller.IndexAsync(page, null, null, null, null, null, null, null, null));
            var items = Data.Items.Entities
                .Skip(take * (page - 1))
                .Take(take);
            int totalCount = Data.Items.Entities.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, take),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithTitleAsync()
        {
            int page = 1;
            int take = 50;

            var actual = (await Controller.IndexAsync(null, null, "sa", null, null, null, null, null, null));
            var items = Data.Items.Entities.Where(i => i.Title.Contains("Sa"));
            int totalCount = items.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, take),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithAllFiltersAsync()
        {
            var samsung7 = Data.Items.Samsung7;

            int page = 1;
            int pageSize = 5;
            var query = Data.Items.Entities
                .Where(i => i.Title.Contains("Samsung"))
                .Where(i => i.StoreId == samsung7.StoreId)
                .Where(i => i.BrandId == samsung7.BrandId);
            int totalCount = query.Count();
            var items = query
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            var expected = new IndexViewModel<ItemViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemViewModel(i));
            var actual = (await Controller.IndexAsync(page, pageSize, "Samsung", samsung7.CategoryId, samsung7.StoreId, samsung7.BrandId, null, null, null));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithAllFiltersThumbnailsAsync()
        {
            var samsung7 = Data.Items.Samsung7;

            int page = 1;
            int pageSize = 5;
            var query = GetQueryable()
                .Where(i => i.Title.Contains("Samsung"))
                .Where(i => i.StoreId == samsung7.StoreId)
                .Where(i => i.BrandId == samsung7.BrandId);
            int totalCount = query.Count();
            var items = query
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            var expected = new IndexViewModel<ItemThumbnailViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemThumbnailViewModel(i));
            var actual = (await Controller.IndexThumbnailsAsync(page, pageSize, "Samsung", samsung7.CategoryId, samsung7.StoreId, samsung7.BrandId, null, null, null));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByCategoryAsync()
        {
            var clothesCategory = Data.Categories.Clothes;
            var categories = new HashSet<Category>();
            await GetCategorySubcategoriesAsync(clothesCategory.Id, categories);
            //var categories =  //await CategoryService.EnumerateSubcategoriesAsync(new CategoryDetailSpecification(clothesCategory.Id));
            int page = 1;
            int pageSize = 5;
            var query = Context.Set<Item>()
                .Where(i => categories.Any(c => c.Id == i.CategoryId));
            int totalCount = query.Count();
            var items = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();
            var expected = new IndexViewModel<ItemViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemViewModel(i));
            var actual = (await Controller.IndexAsync(null, null, null, clothesCategory.Id, null, null, null, null, null));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexPagedWithPriceOrderingAsync()
        {
            int page = 2;
            int pageSize = 2;
            var ordered = Context
                .Set<Item>()
                .AsNoTracking()
                .Include(i => i.ItemVariants)
                .OrderBy(i => (from v in i.ItemVariants select v.Price).Min());

            var actual = (await Controller.IndexAsync(page, pageSize, null, null, null, null, "price", null, null));
            var items = Context
                .Set<Item>()
                .AsNoTracking()
                .OrderBy(i => (from v in i.ItemVariants select v.Price).Min())
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = Data.Items.Entities.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexPagedWithPriceOrderingDescAsync()
        {
            int page = 2;
            int pageSize = 2;
            var ordered = Context
                .Set<Item>()
                .AsNoTracking()
                .Include(i => i.ItemVariants)
                .OrderByDescending(i => (from v in i.ItemVariants select v.Price).Min());

            var actual = (await Controller.IndexAsync(page, pageSize, null, null, null, null, "price", null, true));
            var items = Context
                .Set<Item>()
                .AsNoTracking()
                .OrderByDescending(i => (from v in i.ItemVariants select v.Price).Min())
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = Data.Items.Entities.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexPagedWithTitleOrderingAsync()
        {
            int page = 2;
            int pageSize = 2;
            var ordered = Context
                .Set<Item>()
                .AsNoTracking()
                .Include(i => i.ItemVariants)
                .OrderBy(i => i.Title);

            var actual = (await Controller.IndexAsync(page, pageSize, null, null, null, null, "title", null, null));
            var items = Context
                .Set<Item>()
                .AsNoTracking()
                .OrderBy(i => i.Title)
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = Data.Items.Entities.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task ReadDetailAsync()
        {
            var expectedItem = await GetQueryable()
                .FirstOrDefaultAsync(i => i.Id == Data.Items.Samsung7.Id);
            var expected = new ItemDetailViewModel(expectedItem);
            var actual = await Controller.ReadDetailAsync(Data.Items.Samsung7.Id);
            Equal(expected, actual);
        }


        protected override IQueryable<Item> GetQueryable()
        {
            return Context
                .Set<Item>()
                
                .Include(i => i.Brand)
                .Include(i => i.Store)
                .Include(i => i.MeasurementUnit)
                .Include(i => i.Images)
                .Include(i => i.Category)
                .Include(i => i.ItemVariants)
                    .ThenInclude(j => j.ItemProperties)
                        .ThenInclude(o => o.CharacteristicValue)
                            .ThenInclude(c => c.Characteristic);
        }

        protected override IEnumerable<Item> GetCorrectEntitiesToCreate()
        {
            return new List<Item>()
            {
                new Item()
                {
                    BrandId = Data.Brands.Samsung.Id,
                    CategoryId = Data.Categories.Smartphones.Id,
                    Description = "desc0",
                    MeasurementUnitId = Data.MeasurementUnits.Pcs.Id,
                    StoreId = Data.Stores.Johns.Id,
                    Title = "new item"
                }
            };
        }

        protected override ItemViewModel ToViewModel(Item entity)
        {
            return new ItemViewModel()
            {
                Id = entity.Id,
                StoreId = entity.StoreId,
                MeasurementUnitId = entity.MeasurementUnitId,
                CategoryId = entity.CategoryId,
                BrandId = entity.BrandId,
                Description = entity.Description,
                Title = entity.Title
            };
        }

        protected override IEnumerable<ItemViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<ItemViewModel>()
            {
                new ItemViewModel()
                {
                    Id = Data.Items.Samsung8.Id,
                    BrandId = 9999,
                    CategoryId = 898,
                    MeasurementUnitId = 32,
                    StoreId = 89,
                    Description = "UPDATED",
                    Title = "UPDATED"
                }
            };
        }

        protected override void AssertUpdateSuccess(Item beforeUpdate, ItemViewModel expected, ItemViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);

            Assert.Equal(beforeUpdate.BrandId, actual.BrandId);
            Assert.Equal(beforeUpdate.CategoryId, actual.CategoryId);
            Assert.Equal(beforeUpdate.StoreId, actual.StoreId);
            Assert.Equal(beforeUpdate.MeasurementUnitId, actual.MeasurementUnitId);
        }
    }
}
