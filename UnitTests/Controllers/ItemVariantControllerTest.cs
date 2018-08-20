using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ItemVariantControllerTest : ControllerTestBase<ItemVariant, ItemVariantsController, IItemVariantService>
    {
        public ItemVariantControllerTest(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task IndexByItem()
        {
            int itemId = 1;
            var variants = await GetQueryable(context).Where(v => v.ItemId == itemId).ToListAsync();
            var expected = new ItemVariantIndexViewModel(1, 1, variants.Count, variants);
            var actual = await controller.Index(itemId);
            Assert.Equal(
                    JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                    JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task Post()
        {
            var variant = await GetQueryable(context).FirstOrDefaultAsync();
            foreach (var property in variant.ItemVariantCharacteristicValues)
            {
                property.ItemVariantId = 0;
            }
            variant.Id = 0;
            var viewModel = new ItemVariantViewModel(variant);
            var actual = await controller.Post(viewModel);
            var expected = new ItemVariantViewModel(await GetQueryable(context).FirstOrDefaultAsync(v => v.Id == actual.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task GetDetail()
        {
            var expected = new ItemVariantDetailViewModel(await GetQueryable(context).FirstOrDefaultAsync());
            var actual = await controller.GetDetail(expected.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));

        }
        [Fact]
        public async Task Get()
        {
            var expected = new ItemVariantViewModel(await GetQueryable(context).FirstOrDefaultAsync());
            var actual = await controller.Get(expected.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Put()
        {
            var item = await context.Items.LastOrDefaultAsync();
            var itemVariantViewModel = new ItemVariantViewModel(await GetQueryable(context).FirstOrDefaultAsync(v => v.ItemId == v.Id));
            var categoryProperties = await context
                .Characteristics
                .AsNoTracking()
                .Include(c => c.CharacteristicValues)
                .Where(c => c.CategoryId == item.CategoryId)
                .ToListAsync();
            var actual = await controller.Put(itemVariantViewModel.Id, itemVariantViewModel);
            var expected = new ItemVariantViewModel(await GetQueryable(context).FirstOrDefaultAsync(v => v.Id == actual.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task Delete()
        {
            var variant = await GetQueryable(context).FirstOrDefaultAsync();
            await controller.Delete(variant.Id);
            foreach (var property in variant.ItemVariantCharacteristicValues)
            {
                Assert.False(await context.ItemVariantCharacteristicValues.ContainsAsync(property));
            }
            Assert.False(await context.ItemVariants.ContainsAsync(variant));
        }
        protected override IQueryable<ItemVariant> GetQueryable(StoreContext context)
        {
            return context
                    .ItemVariants
                    .AsNoTracking()
                    .Include(j => j.ItemVariantCharacteristicValues);
        }
    }
}
