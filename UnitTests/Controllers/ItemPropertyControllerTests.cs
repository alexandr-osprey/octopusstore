using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ItemPropertyControllerTests
       : ControllerTestBase<ItemProperty, ItemPropertiesController, IItemPropertyService>
    {
        public ItemPropertyControllerTests(ITestOutputHelper output): base(output)
        {
        }

        [Fact]
        public async Task IndexByVariant()
        {
            var item = await _context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            int variantId = item.ItemVariants.ElementAt(0).Id;
            var values = await GetQueryable().Where(v => v.ItemVariantId == variantId).ToListAsync();
            var expected = new IndexViewModel<ItemPropertyViewModel>(1, 1, values.Count(), from v in values select new ItemPropertyViewModel(v));
            var actual = await controller.Index(variantId, null);
            Equal(expected, actual);
        }
        [Fact]
        public async Task IndexByItem()
        {

            var item = await _context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            var values = await GetQueryable().Where(v => item.ItemVariants.Contains(v.ItemVariant)).ToListAsync();
            var expected = new IndexViewModel<ItemPropertyViewModel>(1, 1, values.Count(), from v in values select new ItemPropertyViewModel(v));
            var actual = await controller.Index(null, item.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task Post()
        {
            var gb32 = await _context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "32GB");
            var xxl = await _context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "XXL");

            var variantWith32gb = await GetQueryable()
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.CharacteristicValueId == gb32.Id);
            variantWith32gb.Id = 0;
            var itemViewModel = new ItemPropertyViewModel(variantWith32gb);
            var actual = await controller.Post(itemViewModel);
            var expected = new ItemPropertyViewModel(
                    await GetQueryable()
                        .FirstOrDefaultAsync(i => i.Id == actual.Id));
            Equal(expected, actual);

            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Post(
                new ItemPropertyViewModel()
                {
                    CharacteristicValueId = xxl.Id,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Post(
                new ItemPropertyViewModel()
                {
                    CharacteristicValueId = 999,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Post(
                new ItemPropertyViewModel()
                {
                    CharacteristicValueId = variantWith32gb.CharacteristicValueId,
                    ItemVariantId = 999
                }));
        }
        [Fact]
        public async Task Put()
        {
            var gb32 = await _context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "32GB");
            var gb64 = await _context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "64GB");
            var xxl = await _context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "XXL");

            var variantWith32gb = await GetQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.CharacteristicValueId == gb32.Id);
            variantWith32gb.CharacteristicValueId = gb64.Id;
            var itemViewModel = new ItemPropertyViewModel(variantWith32gb);
            var actual = await controller.Put(itemViewModel.Id, itemViewModel);
            var expValue = await GetQueryable()
                .FirstOrDefaultAsync(i => i.Id == variantWith32gb.Id);
            var expected = new ItemPropertyViewModel(expValue);

            Equal(expected, actual);

            itemViewModel.Id = 9999;
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Put(itemViewModel.Id, itemViewModel));

            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Put(
                variantWith32gb.Id,
                new ItemPropertyViewModel()
                {
                    CharacteristicValueId = xxl.Id,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Put(
                variantWith32gb.Id,
                new ItemPropertyViewModel()
                {
                    CharacteristicValueId = 999,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            var updated = await controller.Put(
                variantWith32gb.Id,
                new ItemPropertyViewModel()
                {
                    CharacteristicValueId = variantWith32gb.CharacteristicValueId,
                    ItemVariantId = 999
                });
            Assert.Equal(updated.ItemVariantId, variantWith32gb.ItemVariantId);
        }
        [Fact]
        public async Task PutNew()
        {
            var firstValue = await GetQueryable()
           .AsNoTracking()
           .FirstOrDefaultAsync();
            firstValue.CharacteristicValueId = firstValue.CharacteristicValueId + 1;
            firstValue.Id = 0;
            var itemViewModel = new ItemPropertyViewModel(firstValue);
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Put(itemViewModel.Id, itemViewModel));
        }
        [Fact]
        public async Task Delete()
        {
            int id = 3;
            await controller.Delete(id);
            Assert.False(_context.ItemProperties.Where(i => i.Id == id).Any());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Delete(9999));
        }
        protected override IQueryable<ItemProperty> GetQueryable()
        {
            return _context
                .Set<ItemProperty>()
                .Include(i => i.CharacteristicValue)
                    .ThenInclude(i => i.Characteristic);
        }
    }
}
