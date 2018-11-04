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
            int variantId = Data.ItemVariants.IPhone664GB.Id;
            var values = await GetQueryable().Where(v => v.ItemVariantId == variantId).ToListAsync();
            var expected = new IndexViewModel<ItemPropertyViewModel>(1, 1, values.Count(), from v in values select new ItemPropertyViewModel(v));
            var actual = await Controller.IndexAsync(variantId, null);
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexByItemAsync()
        {
            var item = Data.Items.IPhone6;
            var itemVariants = Data.ItemVariants.Entities.Where(i => i.ItemId == item.Id);
            var values = Data.ItemProperties.Entities.Where(i => (from iv in itemVariants select iv.Id).Contains(i.ItemVariantId));
            var expected = new IndexViewModel<ItemPropertyViewModel>(1, 1, values.Count(), from v in values select new ItemPropertyViewModel(v));
            var actual = await Controller.IndexAsync(null, item.Id);
            Equal(expected, actual);
        }

        protected override IQueryable<ItemProperty> GetQueryable()
        {
            return Context
                .Set<ItemProperty>()
                .Include(i => i.CharacteristicValue)
                    .ThenInclude(i => i.Characteristic);
        }

        protected override IEnumerable<ItemProperty> GetCorrectEntitiesToCreate()
        {
            return new List<ItemProperty>()
            {
                new ItemProperty() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, CharacteristicValueId = Data.CharacteristicValues.MuchFashion.Id },
                new ItemProperty() { ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id, CharacteristicValueId = Data.CharacteristicValues.GB16.Id },
            };
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

        protected override IEnumerable<ItemProperty> GetCorrectEntitiesToUpdate()
        {
            var entities = Data.ItemProperties.Entities.Take(5).ToList();
            entities.ForEach(e => 
            {
                e.CharacteristicValueId = 999;
                e.ItemVariantId = 999;
            });
            return entities;
        }

        protected override void AssertUpdateSuccess(ItemProperty beforeUpdate, ItemPropertyViewModel expected, ItemPropertyViewModel actual)
        {
            Assert.Equal(beforeUpdate.Id, actual.Id);
            Assert.Equal(beforeUpdate.ItemVariantId, actual.ItemVariantId);
            Assert.Equal(beforeUpdate.CharacteristicValueId, actual.CharacteristicValueId);
        }
    }
}
