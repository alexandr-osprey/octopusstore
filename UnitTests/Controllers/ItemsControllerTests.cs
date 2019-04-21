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

            var actual = (await _controller.IndexAsync(page, pageSize, null, null, null, null, null, null, null));
            var items = _data.Items.Entities
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = _data.Items.Entities.Count();
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
                _data.CharacteristicValues.SmartphoneBattery3000,
                _data.CharacteristicValues.SmartphoneResolutionFullHD
            };
            string filter = string.Join(';', (from c in characteristicValues select c.Id.ToString()));
            var actual = (await _controller.IndexAsync(page, pageSize, null, null, null, null, null, filter, null));
            var items = _data.Items.Entities.Where(i => characteristicValues.IsSubsetOf(from p in i.ItemVariants.SelectMany(v => v.ItemProperties) select p.CharacteristicValue));
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
                _data.CharacteristicValues.SmartphoneResolutionFullHD,
                _data.CharacteristicValues.SmartphoneBattery4000
            };
            string filter = string.Join(';', (from c in characteristicValues select c.Id.ToString()));
            var actual = (await _controller.IndexThumbnailsAsync(page, pageSize, null, null, null, null, null, filter, null));
            var items = _data.Items.Entities.Where(i => characteristicValues.IsSubsetOf(from p in i.ItemVariants.SelectMany(v => v.ItemProperties) select p.CharacteristicValue));
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

            var emptyActual = (await _controller.IndexAsync(page, pageSize, null, null, null, null, null, "9999;8888", null));
            Assert.Empty(emptyActual.Entities);

            HashSet<CharacteristicValue> wrongCharacteristicValues = new HashSet<CharacteristicValue>
            {
                _data.CharacteristicValues.WomensAccessoryColorBlack,
                _data.CharacteristicValues.SmartphoneRAM8
            };
            string filter = string.Join(';', (from c in wrongCharacteristicValues select c.Id.ToString()));

            var emptyActual2 = (await _controller.IndexAsync(page, pageSize, null, null, null, null, null, filter, null));
            Assert.Empty(emptyActual2.Entities);
        }

        [Fact]
        public async Task IndexWithoutFiltersWithoutPageSizeAsync()
        {
            int page = 2;
            int take = 50;

            var actual = (await _controller.IndexAsync(page, null, null, null, null, null, null, null, null));
            var items = _data.Items.Entities
                .Skip(take * (page - 1))
                .Take(take);
            int totalCount = _data.Items.Entities.Count();
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

            var actual = (await _controller.IndexAsync(null, null, "sa", null, null, null, null, null, null));
            var items = _data.Items.Entities.Where(i => i.Title.Contains("Sa"));
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
            var samsung10 = _data.Items.SamsungS10;

            int page = 1;
            int pageSize = 5;
            var query = _data.Items.Entities
                .Where(i => i.Title.Contains("Samsung"))
                .Where(i => i.StoreId == samsung10.StoreId)
                .Where(i => i.CategoryId == samsung10.CategoryId)
                .Where(i => i.BrandId == samsung10.BrandId);
            int totalCount = query.Count();
            var items = query
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            var expected = new IndexViewModel<ItemViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemViewModel(i));
            var actual = (await _controller.IndexAsync(page, pageSize, "Samsung", samsung10.CategoryId, samsung10.StoreId, samsung10.BrandId, null, null, null));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithAllFiltersThumbnailsAsync()
        {
            var samsung10 = _data.Items.SamsungS10;

            int page = 1;
            int pageSize = 5;
            var query = GetQueryable()
                .Where(i => i.Title.Contains("Samsung"))
                .Where(i => i.StoreId == samsung10.StoreId)
                .Where(i => i.CategoryId == samsung10.CategoryId)
                .Where(i => i.BrandId == samsung10.BrandId);
            int totalCount = query.Count();
            var items = query
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            var expected = new IndexViewModel<ItemThumbnailViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemThumbnailViewModel(i));
            var actual = (await _controller.IndexThumbnailsAsync(page, pageSize, "Samsung", samsung10.CategoryId, samsung10.StoreId, samsung10.BrandId, null, null, null));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByCategoryAsync()
        {
            var clothesCategory = _data.Categories.WomensClothing;
            var categories = new HashSet<Category>();
            await GetCategorySubcategoriesAsync(clothesCategory.Id, categories);
            //var categories =  //await CategoryService.EnumerateSubcategoriesAsync(new CategoryDetailSpecification(clothesCategory.Id));
            int page = 1;
            int pageSize = 5;
            var query = _context.Set<Item>()
                .Where(i => categories.Any(c => c.Id == i.CategoryId));
            int totalCount = query.Count();
            var items = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();
            var expected = new IndexViewModel<ItemViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemViewModel(i));
            var actual = (await _controller.IndexAsync(null, pageSize, null, clothesCategory.Id, null, null, null, null, null));
            actual.Entities = actual.Entities.OrderBy(i => i.Id);
            expected.Entities = expected.Entities.OrderBy(i => i.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexPagedWithPriceOrderingAsync()
        {
            int page = 2;
            int pageSize = 2;
            var ordered = _context
                .Set<Item>()
                .AsNoTracking()
                .Include(i => i.ItemVariants)
                .OrderBy(i => (from v in i.ItemVariants select v.Price).Min());

            var actual = (await _controller.IndexAsync(page, pageSize, null, null, null, null, "price", null, null));
            var items = _context
                .Set<Item>()
                .AsNoTracking()
                .OrderBy(i => (from v in i.ItemVariants select v.Price).Min())
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = _data.Items.Entities.Count();
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

            var actual = (await _controller.IndexAsync(page, pageSize, null, null, null, null, "price", null, true));
            var items = _context
                .Set<Item>()
                .OrderByDescending(i => (from v in i.ItemVariants select v.Price).Min())
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = _data.Items.Entities.Count();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            actual.Entities = actual.Entities.OrderBy(i => i.Id);
            expected.Entities = expected.Entities.OrderBy(i => i.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexPagedWithTitleOrderingAsync()
        {
            int page = 2;
            int pageSize = 2;
            var ordered = _context
                .Set<Item>()
                .AsNoTracking()
                .Include(i => i.ItemVariants)
                .OrderBy(i => i.Title);

            var actual = (await _controller.IndexAsync(page, pageSize, null, null, null, null, "title", null, null));
            var items = _context
                .Set<Item>()
                .AsNoTracking()
                .OrderBy(i => i.Title)
                .Skip(pageSize * (page - 1))
                .Take(pageSize);
            int totalCount = _data.Items.Entities.Count();
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
                .FirstOrDefaultAsync(i => i.Id == _data.Items.SamsungS10.Id);
            var expected = new ItemDetailViewModel(expectedItem);
            var actual = await _controller.ReadDetailAsync(_data.Items.SamsungS10.Id);
            Equal(expected, actual);
        }


        protected override IQueryable<Item> GetQueryable()
        {
            return _context
                .Set<Item>()
                
                .Include(i => i.Brand)
                .Include(i => i.Store)
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
                    BrandId = _data.Brands.Samsung.Id,
                    CategoryId = _data.Categories.Smartphones.Id,
                    Description = "desc0",
                    StoreId = _data.Stores.Johns.Id,
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
                    Id = _data.Items.SamsungS9.Id,
                    BrandId = 9999,
                    CategoryId = 898,
                    MeasurementUnitId = 32,
                    StoreId = 89,
                    Description = "UPDATED",
                    Title = "UPDATED"
                }
            };
        }

        protected override Task AssertUpdateSuccessAsync(Item beforeUpdate, ItemViewModel expected, ItemViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);

            Assert.Equal(beforeUpdate.BrandId, actual.BrandId);
            Assert.Equal(beforeUpdate.CategoryId, actual.CategoryId);
            Assert.Equal(beforeUpdate.StoreId, actual.StoreId);
            return Task.CompletedTask;
        }
    }
}
