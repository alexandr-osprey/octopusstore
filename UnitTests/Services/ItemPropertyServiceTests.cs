using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class ItemPropertyServiceTests: ServiceTests<ItemProperty, IItemPropertyService>
    {
        public ItemPropertyServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        [Fact]
        public async Task CreateAsyncThrowsAlreadyExists()
        {
            foreach (var duplicateEntity in await GetDuplicateEntitiesAsync())
            {
                await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.CreateAsync(duplicateEntity));
            }
        }

        protected override IEnumerable<ItemProperty> GetCorrectNewEntites()
        {
            return new List<ItemProperty>()
            {
                //new ItemProperty() { ItemVariantId = _data.ItemVariants.DanielePatriciBag1.Id, CharacteristicValueId = _data.CharacteristicValues.MuchFashion.Id },
                //new ItemProperty() { ItemVariantId = _data.ItemVariants.DanielePatriciBag2.Id, CharacteristicValueId = _data.CharacteristicValues.SmartphoneStorage16GB.Id },
            };
        }

        protected override IEnumerable<ItemProperty> GetIncorrectNewEntites()
        {
            var iphone664gb = _data.ItemProperties.Entities.FirstOrDefault(i => i.ItemVariant == _data.ItemVariants.IPhone8Plus64GBBlack);
            return new List<ItemProperty>()
            {
                new ItemProperty() {  ItemVariantId = iphone664gb.ItemVariantId, CharacteristicValueId = 999, OwnerId = iphone664gb.OwnerId },
                new ItemProperty() {  ItemVariantId = iphone664gb.ItemVariantId, CharacteristicValueId = _data.CharacteristicValues.WomensDressColorWhite.Id, OwnerId = iphone664gb.OwnerId },
                new ItemProperty() {  ItemVariantId = 999, CharacteristicValueId = iphone664gb.CharacteristicValueId, OwnerId = iphone664gb.OwnerId },
            };
        }

        protected override Specification<ItemProperty> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<ItemProperty>(i => i == _data.ItemProperties.Entities.LastOrDefault());
        }

        protected override IEnumerable<ItemProperty> GetCorrectEntitesForUpdate()
        {
            return new List<ItemProperty>() { };
        }

        protected override IEnumerable<ItemProperty> GetIncorrectEntitesForUpdate()
        {
            var iphone664gb = _data.ItemProperties.IPhoneXR64GBWhiteBattery3000;
            var shoesDMS = _data.ItemProperties.ReebokFastTempoWhite35White;
            iphone664gb.CharacteristicValueId = _data.CharacteristicValues.WomensFootwearTypeShoes.Id;
            shoesDMS.ItemVariantId = _data.ItemVariants.SamsungGalaxyS1064GBWhite.Id;
            return new List<ItemProperty>()
            {
                iphone664gb,
                shoesDMS
            };
        }

        protected async Task<IEnumerable<ItemProperty>> GetDuplicateEntitiesAsync()
        {
            var iphone664gb = _data.ItemProperties.Entities.FirstOrDefault(i => i.ItemVariant == _data.ItemVariants.IPhone8Plus64GBBlack);
            return await Task.FromResult(new List<ItemProperty>
            {
                new ItemProperty() {  CharacteristicValueId = iphone664gb.CharacteristicValueId, ItemVariantId = iphone664gb.ItemVariantId },
            });
        }
    }
}
