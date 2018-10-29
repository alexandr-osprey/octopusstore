using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ItemVariantControllerTest : ControllerTestBase<ItemVariant, ItemVariantsController, IItemVariantService>
    {
        public ItemVariantControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexByItem()
        {
            int itemId = 1;
            var variants = await GetQueryable().Where(v => v.ItemId == itemId).ToListAsync();
            var expected = new IndexViewModel<ItemVariantViewModel>(1, 1, variants.Count, from v in variants select new ItemVariantViewModel(v));
            var actual = await controller.Index(itemId);
            Equal(expected, actual);
        }
        [Fact]
        public async Task Post()
        {
            var variant = await GetQueryable().FirstOrDefaultAsync();
            foreach (var property in variant.ItemProperties)
            {
                property.ItemVariantId = 0;
            }
            variant.Id = 0;
            var viewModel = new ItemVariantViewModel(variant);
            var actual = await controller.Post(viewModel);
            var expected = new ItemVariantViewModel(await GetQueryable().FirstOrDefaultAsync(v => v.Id == actual.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task GetDetail()
        {
            var expected = new ItemVariantDetailViewModel(await GetQueryable().FirstOrDefaultAsync());
            var actual = await controller.GetDetail(expected.Id);
            Equal(expected, actual);
        }
        [Fact]
        public async Task Get()
        {
            var expected = new ItemVariantViewModel(await GetQueryable().FirstOrDefaultAsync());
            var actual = await controller.Get(expected.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task Put()
        {
            var item = await _context.Items.LastOrDefaultAsync();
            var itemVariantViewModel = new ItemVariantViewModel(await GetQueryable().FirstOrDefaultAsync(v => v.ItemId == v.Id));
            var categoryProperties = await _context
                .Characteristics
                .AsNoTracking()
                .Include(c => c.CharacteristicValues)
                .Where(c => c.CategoryId == item.CategoryId)
                .ToListAsync();
            var actual = await controller.Put(itemVariantViewModel.Id, itemVariantViewModel);
            var expected = new ItemVariantViewModel(await GetQueryable().FirstOrDefaultAsync(v => v.Id == actual.Id));
            Equal(expected, actual);
        }
        [Fact]
        public async Task Delete()
        {
            var variant = await GetQueryable().FirstOrDefaultAsync();
            await controller.Delete(variant.Id);
            foreach (var property in variant.ItemProperties)
            {
                Assert.False(await _context.ItemProperties.ContainsAsync(property));
            }
            Assert.False(await _context.ItemVariants.ContainsAsync(variant));
        }
        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return _context
                    .Set<ItemVariant>()
                    .AsNoTracking()
                    .Include(j => j.ItemProperties);
        }
    }
}
