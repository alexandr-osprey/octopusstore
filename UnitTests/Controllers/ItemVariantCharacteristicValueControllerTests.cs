using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using OctopusStore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ItemVariantCharacteristicValueControllerTests
        : ControllerTestBase<ItemVariantCharacteristicValue, ItemVariantCharacteristicValueController, IItemVariantCharacteristicValueService>
    {
        public ItemVariantCharacteristicValueControllerTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task IndexByVariant()
        {
            var item = await context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            int variantId = item.ItemVariants.ElementAt(0).Id;
            var values = await GetQueryable(context).Where(v => v.ItemVariantId == variantId).ToListAsync();
            var expected = new ItemVariantCharacteristicValueIndexViewModel(1, 1, values.Count, values);
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
            var expected = new ItemVariantCharacteristicValueIndexViewModel(1, 1, values.Count, values);
            var actual = await controller.Index(null, item.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Post()
        {
            var firstValue = await GetQueryable(context)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            firstValue.Id = 0;
            var itemViewModel = new ItemVariantCharacteristicValueViewModel(firstValue);
            var actual = await controller.Post(itemViewModel);
            var expected = new ItemVariantCharacteristicValueViewModel(
                    await GetQueryable(context)
                        .FirstOrDefaultAsync(i => i.Id == actual.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task Put()
        {
            var firstValue = await GetQueryable(context)
               .AsNoTracking()
               .FirstOrDefaultAsync();
            firstValue.CharacteristicValueId = firstValue.CharacteristicValueId + 1;
            var itemViewModel = new ItemVariantCharacteristicValueViewModel(firstValue);
            var actual = await controller.Put(itemViewModel.Id, itemViewModel);
            var expValue = await GetQueryable(context)
                .FirstOrDefaultAsync(i => i.Id == firstValue.Id);
            var expected = new ItemVariantCharacteristicValueViewModel(expValue);

            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));

            itemViewModel.Id = 9999;
            await Assert.ThrowsAsync<EntityNotFoundException>(() => controller.Put(itemViewModel.Id, itemViewModel));
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
        protected override IQueryable<ItemVariantCharacteristicValue> GetQueryable(StoreContext context)
        {
            return context
                .ItemVariantCharacteristicValues
                .Include(i => i.CharacteristicValue)
                    .ThenInclude(i => i.Characteristic);
        }
    }
}
