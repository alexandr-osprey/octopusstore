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
    public class ItemVariantCharacteristicValueControllerTests
        : ControllerTestBase<ItemVariantCharacteristicValue, ItemVariantCharacteristicValuesController, IItemVariantCharacteristicValueService>
    {
        public ItemVariantCharacteristicValueControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexByVariant()
        {
            var item = await context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            int variantId = item.ItemVariants.ElementAt(0).Id;
            var values = await GetQueryable(context).Where(v => v.ItemVariantId == variantId).ToListAsync();
            var expected = new IndexViewModel<ItemVariantCharacteristicValueViewModel>(1, 1, values.Count(), from v in values select new ItemVariantCharacteristicValueViewModel(v));
            var actual = await controller.Index(variantId, null);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task IndexByItem()
        {

            var item = await context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            var values = await GetQueryable(context).Where(v => item.ItemVariants.Contains(v.ItemVariant)).ToListAsync();
            var expected = new IndexViewModel<ItemVariantCharacteristicValueViewModel>(1, 1, values.Count(), from v in values select new ItemVariantCharacteristicValueViewModel(v));
            var actual = await controller.Index(null, item.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Post()
        {
            var gb32 = await context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "32GB");
            var xxl = await context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "XXL");

            var variantWith32gb = await GetQueryable(context)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.CharacteristicValueId == gb32.Id);
            variantWith32gb.Id = 0;
            var itemViewModel = new ItemVariantCharacteristicValueViewModel(variantWith32gb);
            var actual = await controller.Post(itemViewModel);
            var expected = new ItemVariantCharacteristicValueViewModel(
                    await GetQueryable(context)
                        .FirstOrDefaultAsync(i => i.Id == actual.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));

            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Post(
                new ItemVariantCharacteristicValueViewModel()
                {
                    CharacteristicValueId = xxl.Id,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Post(
                new ItemVariantCharacteristicValueViewModel()
                {
                    CharacteristicValueId = 999,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Post(
                new ItemVariantCharacteristicValueViewModel()
                {
                    CharacteristicValueId = variantWith32gb.CharacteristicValueId,
                    ItemVariantId = 999
                }));
        }
        [Fact]
        public async Task Put()
        {
            var gb32 = await context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "32GB");
            var gb64 = await context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "64GB");
            var xxl = await context
                .Set<CharacteristicValue>()
                .FirstAsync(c => c.Title == "XXL");

            var variantWith32gb = await GetQueryable(context)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.CharacteristicValueId == gb32.Id);
            variantWith32gb.CharacteristicValueId = gb64.Id;
            var itemViewModel = new ItemVariantCharacteristicValueViewModel(variantWith32gb);
            var actual = await controller.Put(itemViewModel.Id, itemViewModel);
            var expValue = await GetQueryable(context)
                .FirstOrDefaultAsync(i => i.Id == variantWith32gb.Id);
            var expected = new ItemVariantCharacteristicValueViewModel(expValue);

            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));

            itemViewModel.Id = 9999;
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Put(itemViewModel.Id, itemViewModel));

            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Put(
                variantWith32gb.Id,
                new ItemVariantCharacteristicValueViewModel()
                {
                    CharacteristicValueId = xxl.Id,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            await Assert.ThrowsAsync<EntityValidationException>(() => controller.Put(
                variantWith32gb.Id,
                new ItemVariantCharacteristicValueViewModel()
                {
                    CharacteristicValueId = 999,
                    ItemVariantId = variantWith32gb.ItemVariantId
                }));
            var updated = await controller.Put(
                variantWith32gb.Id,
                new ItemVariantCharacteristicValueViewModel()
                {
                    CharacteristicValueId = variantWith32gb.CharacteristicValueId,
                    ItemVariantId = 999
                });
            Assert.Equal(updated.ItemVariantId, variantWith32gb.ItemVariantId);
        }
        [Fact]
        public async Task PutNew()
        {
            var firstValue = await GetQueryable(context)
           .AsNoTracking()
           .FirstOrDefaultAsync();
            firstValue.CharacteristicValueId = firstValue.CharacteristicValueId + 1;
            firstValue.Id = 0;
            var itemViewModel = new ItemVariantCharacteristicValueViewModel(firstValue);
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Put(itemViewModel.Id, itemViewModel));
        }
        [Fact]
        public async Task Delete()
        {
            int id = 3;
            await controller.Delete(id);
            Assert.False(context.ItemVariantCharacteristicValues.Where(i => i.Id == id).Any());

            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Delete(9999));
        }
        protected override IQueryable<ItemVariantCharacteristicValue> GetQueryable(DbContext context)
        {
            return context
                .Set<ItemVariantCharacteristicValue>()
                .Include(i => i.CharacteristicValue)
                    .ThenInclude(i => i.Characteristic);
        }
    }
}
