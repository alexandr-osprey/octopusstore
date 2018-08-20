using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class ItemVariantServiceTests : ServiceTestBase<ItemVariant, IItemVariantService>
    {
        public ItemVariantServiceTests(ITestOutputHelper output)
            : base(output)
        { }

        [Fact]
        public async Task AddAsync()
        {
            var variant = new ItemVariant() { Price = 100, ItemId = 1, Title = "added" };
            variant.ItemVariantCharacteristicValues = new List<ItemVariantCharacteristicValue>();
            variant.ItemVariantCharacteristicValues.Add(new ItemVariantCharacteristicValue() { CharacteristicValueId = 2 });
            variant.ItemVariantCharacteristicValues.Add(new ItemVariantCharacteristicValue() { CharacteristicValueId = 4 });
            await service.AddAsync(variant);
            Assert.True(context.ItemVariants.Contains(variant));
            Assert.True(context.ItemVariantCharacteristicValues.Contains(variant.ItemVariantCharacteristicValues.ElementAt(1)));

            variant.Id = 0;
            variant.ItemVariantCharacteristicValues = null;
            await service.AddAsync(variant);
            Assert.True(context.ItemVariants.Contains(variant));
            Assert.True(await context.ItemVariantCharacteristicValues.AnyAsync(i => i.ItemVariantId == variant.Id));
        }
        [Fact]
        public async Task UpdateAsync()
        {
            var expected = await GetQueryable(context).FirstOrDefaultAsync();
            expected.Title = "new title";
            expected.ItemVariantCharacteristicValues.Remove(expected.ItemVariantCharacteristicValues.Last());
            await service.UpdateAsync(expected);
            expected.ItemVariantCharacteristicValues = await context
                .ItemVariantCharacteristicValues
                .Where(i => i.ItemVariantId == expected.Id).ToListAsync();
            foreach (var v in expected.ItemVariantCharacteristicValues)
            {
                v.ItemVariant = expected;
            }
            var actual = await GetQueryable(context)
                .FirstOrDefaultAsync(i => i.Id == expected.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task DeleteAsync()
        {
            var variant = await GetQueryable(context).LastOrDefaultAsync();
            await service.DeleteAsync(new Specification<ItemVariant>(variant.Id));
            Assert.False(context.ItemVariants.Contains(variant));
            Assert.False(await context.ItemVariantCharacteristicValues.AnyAsync(i => i.ItemVariantId == variant.Id));
        }
        protected override IQueryable<ItemVariant> GetQueryable(StoreContext context)
        {
            return base.GetQueryable(context).Include(i => i.ItemVariantCharacteristicValues);
        }
    }
}
