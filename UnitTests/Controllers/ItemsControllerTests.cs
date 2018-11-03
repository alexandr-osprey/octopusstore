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
        private readonly ICategoryService _categoryService;

        public ItemControllerTests(ITestOutputHelper output) : base(output)
        {
            _categoryService = Resolve<ICategoryService>();
        }

        [Fact]
        public async Task IndexWithoutFilters()
        {
            int page = 2;
            int pageSize = 3;

            var actual = (await _controller.IndexAsync(page, pageSize, null, null, null, null));
            var items = await GetQueryable()
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();
            int totalCount = await _context.Items.CountAsync();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexWithoutFiltersWithoutPageSize()
        {
            int page = 2;
            int take = 50;

            var actual = (await _controller.IndexAsync(page, null, null, null, null, null));
            var items = await GetQueryable()
                .Skip(take * (page - 1))
                .Take(take).ToListAsync();
            int totalCount = await _context.Items.CountAsync();
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, take),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexWithTitle()
        {
            int page = 1;
            int take = 50;

            var actual = (await _controller.IndexAsync(null, null, "sa", null, null, null));
            var items = await GetQueryable().Where(i => i.Title.Contains("Sa")).ToListAsync();
            int totalCount = items.Count;
            var expected = new IndexViewModel<ItemViewModel>(
                page,
                GetPageCount(totalCount, take),
                totalCount,
                from i in items select new ItemViewModel(i));
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexWithAllFilters()
        {
            var samsung7 = await _context.Items
            .AsNoTracking()
            .Where(i => i.Title.Contains("Samsung 7"))
            .FirstOrDefaultAsync();

            int page = 1;
            int pageSize = 5;
            var query = GetQueryable()
                .Where(i => i.Title.Contains("Samsung"))
                .Where(i => i.StoreId == samsung7.StoreId)
                .Where(i => i.BrandId == samsung7.BrandId);
            int totalCount = query.Count();
            var items = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();
            var expected = new IndexViewModel<ItemViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemViewModel(i));
            var actual = (await _controller.IndexAsync(page, pageSize, "Samsung", samsung7.CategoryId, samsung7.StoreId, samsung7.BrandId));
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexWithAllFiltersThumbnails()
        {
            var samsung7 = await _context.Items
            .AsNoTracking()
            .Where(i => i.Title.Contains("Samsung 7"))
            .FirstOrDefaultAsync();

            int page = 1;
            int pageSize = 5;
            var query = GetQueryable()
                .Where(i => i.Title.Contains("Samsung"))
                .Where(i => i.StoreId == samsung7.StoreId)
                .Where(i => i.BrandId == samsung7.BrandId);
            int totalCount = query.Count();
            var items = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();
            var expected = new IndexViewModel<ItemThumbnailViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemThumbnailViewModel(i));
            var actual = (await _controller.IndexThumbnailsAsync(page, pageSize, "Samsung", samsung7.CategoryId, samsung7.StoreId, samsung7.BrandId));
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexByCategory()
        {
            var clothesCategory = await _context.Categories
            .AsNoTracking()
            .Where(c => c.Title == "Clothes")
            .Include(c => c.Subcategories)
            .FirstOrDefaultAsync();

            var categories = await _categoryService.EnumerateSubcategoriesAsync(new CategoryDetailSpecification(clothesCategory.Id));

            int page = 1;
            int pageSize = 5;
            var query = _context.Set<Item>()
                .Where(i => categories.Any(c => c.Id == i.CategoryId));
            int totalCount = query.Count();
            var items = await query
                .Skip(pageSize * (page - 1))
                .Take(pageSize).ToListAsync();
            var expected = new IndexViewModel<ItemViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from i in items select new ItemViewModel(i));
            var actual = (await _controller.IndexAsync(null, null, null, clothesCategory.Id, null, null));
            Equal(expected, actual);
        }

        [Fact]
        public async Task ReadDetail()
        {
            var samsungItem = await _context.Items
                .AsNoTracking()
                .Where(i => i.Title.Contains("Samsung 7"))
                .FirstOrDefaultAsync();
            var expectedItem = await GetQueryable()
                .FirstOrDefaultAsync(i => i.Id == samsungItem.Id);
            var expected = new ItemDetailViewModel(expectedItem);
            var actual = await _controller.ReadDetailAsync(samsungItem.Id);
            Equal(expected, actual);
        }


        protected override IQueryable<Item> GetQueryable()
        {
            return _context
                .Set<Item>()
                .AsNoTracking()
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

        protected override async Task<IEnumerable<Item>> GetCorrectEntitiesToCreateAsync()
        {
            return await Task.FromResult(new List<Item>()
            {
                new Item() { BrandId = 1, CategoryId = 6, Description = "desc0", MeasurementUnitId = 1, StoreId = 1, Title = "new item" }
            });
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

        protected override async Task<IEnumerable<Item>> GetCorrectEntitiesToUpdateAsync()
        {
            var items = await _context.Set<Item>().AsNoTracking().Take(5).ToListAsync();
            items.ForEach(i => 
            {
                i.BrandId = 999;
                i.CategoryId = 999;
                i.Description = "UPDATED";
                i.StoreId = 999;
                i.Title = "UPDATED TITLE";
                i.MeasurementUnitId = 999;
            });
            return items;
        }

        protected override Task AssertUpdateSuccessAsync(Item beforeUpdate, ItemViewModel expected, ItemViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Description, actual.Description);

            Assert.Equal(beforeUpdate.BrandId, actual.BrandId);
            Assert.Equal(beforeUpdate.CategoryId, actual.CategoryId);
            Assert.Equal(beforeUpdate.StoreId, actual.StoreId);
            Assert.Equal(beforeUpdate.MeasurementUnitId, actual.MeasurementUnitId);
            return Task.CompletedTask;
        }
    }
}
