using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => Service.CreateAsync(duplicateEntity));
            }
        }

        protected override IEnumerable<ItemProperty> GetCorrectNewEntites()
        {
            return new List<ItemProperty>()
            {
                new ItemProperty() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, CharacteristicValueId = Data.CharacteristicValues.MuchFashion.Id },
                new ItemProperty() { ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id, CharacteristicValueId = Data.CharacteristicValues.GB16.Id },
            };
        }

        protected override IEnumerable<ItemProperty> GetIncorrectNewEntites()
        {
            var iphone664gb = Data.ItemProperties.IPhone664HD;
            return new List<ItemProperty>()
            {
                new ItemProperty() {  ItemVariantId = iphone664gb.ItemVariantId, CharacteristicValueId = 999, OwnerId = iphone664gb.OwnerId },
                new ItemProperty() {  ItemVariantId = iphone664gb.ItemVariantId, CharacteristicValueId = Data.CharacteristicValues.NotSoFashion.Id, OwnerId = iphone664gb.OwnerId },
                new ItemProperty() {  ItemVariantId = 999, CharacteristicValueId = iphone664gb.CharacteristicValueId, OwnerId = iphone664gb.OwnerId },
            };
        }

        protected override Specification<ItemProperty> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<ItemProperty>(i => i == Data.ItemProperties.Samsung732HDHD);
        }

        protected override IEnumerable<ItemProperty> GetCorrectEntitesForUpdate()
        {
            return new List<ItemProperty>() { };
        }
        
        protected async Task<IEnumerable<ItemProperty>> GetDuplicateEntitiesAsync()
        {
            var iphone = Data.ItemProperties.IPhone632GB32;
            return await Task.FromResult(new List<ItemProperty>
            {
                new ItemProperty() {  CharacteristicValueId = iphone.CharacteristicValueId, ItemVariantId = iphone.ItemVariantId },
            });
        }
    }
}
