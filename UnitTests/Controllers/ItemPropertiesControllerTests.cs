using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
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
    public class ItemPropertyControllerTests: ControllerTests<ItemProperty, ItemPropertyViewModel, IItemPropertiesController, IItemPropertyService>
    {
        public ItemPropertyControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexByVariantAsync()
        {
            var item = await _context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            int variantId = item.ItemVariants.ElementAt(0).Id;
            var values = await GetQueryable().Where(v => v.ItemVariantId == variantId).ToListAsync();
            var expected = new IndexViewModel<ItemPropertyViewModel>(1, 1, values.Count(), from v in values select new ItemPropertyViewModel(v));
            var actual = await _controller.IndexAsync(variantId, null);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByItemAsync()
        {
            var item = await _context.Items.Include(i => i.ItemVariants).FirstOrDefaultAsync();
            var values = await GetQueryable().Where(v => item.ItemVariants.Contains(v.ItemVariant)).ToListAsync();
            var expected = new IndexViewModel<ItemPropertyViewModel>(1, 1, values.Count(), from v in values select new ItemPropertyViewModel(v));
            var actual = await _controller.IndexAsync(null, item.Id);
            Equal(expected, actual);
        }

        protected override IQueryable<ItemProperty> GetQueryable()
        {
            return _context
                .Set<ItemProperty>()
                .Include(i => i.CharacteristicValue)
                    .ThenInclude(i => i.Characteristic);
        }

        protected override async Task<IEnumerable<ItemProperty>> GetCorrectEntitiesToCreateAsync()
        {
            var newVariant = (await _context.Set<ItemVariant>().AddAsync(new ItemVariant() { ItemId = 1, OwnerId = johnId, Price = 500, Title = "title" })).Entity;
            await _context.SaveChangesAsync();
            return await Task.FromResult(new List<ItemProperty>()
            {
                new ItemProperty() { ItemVariantId = newVariant.Id, CharacteristicValueId = 5 },
                new ItemProperty() { ItemVariantId = newVariant.Id, CharacteristicValueId = 1 },
                //new ItemProperty() { ItemVariantId = 9, CharacteristicValueId = 11 }
            });
        }

        protected override ItemPropertyViewModel ToViewModel(ItemProperty entity)
        {
            return new ItemPropertyViewModel()
            {
                Id = entity.Id,
                CharacteristicValueId = entity.CharacteristicValueId,
                ItemVariantId = entity.ItemVariantId
            };
        }

        protected override async Task<IEnumerable<ItemProperty>> GetCorrectEntitiesToUpdateAsync()
        {
            var entities = await _context.Set<ItemProperty>().AsNoTracking().Take(10).ToListAsync();
            entities.ForEach(e => 
            {
                e.CharacteristicValueId = 999;
                e.ItemVariantId = 999;
            });
            return entities;
        }

        protected override Task AssertUpdateSuccessAsync(ItemProperty beforeUpdate, ItemPropertyViewModel expected, ItemPropertyViewModel actual)
        {
            Assert.Equal(beforeUpdate.Id, actual.Id);
            Assert.Equal(beforeUpdate.ItemVariantId, actual.ItemVariantId);
            Assert.Equal(beforeUpdate.CharacteristicValueId, actual.CharacteristicValueId);
            return Task.CompletedTask;
        }
    }
}
