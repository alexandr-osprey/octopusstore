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
                await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.CreateAsync(duplicateEntity));
            }
        }

        protected override IEnumerable<ItemProperty> GetCorrectNewEntites()
        {
            return new List<ItemProperty>()
            {
                new ItemProperty() { ItemVariantId = _data.ItemVariants.JacketBlack.Id, CharacteristicValueId = _data.CharacteristicValues.MuchFashion.Id },
                new ItemProperty() { ItemVariantId = _data.ItemVariants.Pebble1000mAh.Id, CharacteristicValueId = _data.CharacteristicValues.GB16.Id },
            };
        }

        protected override IEnumerable<ItemProperty> GetIncorrectNewEntites()
        {
            var iphone664gb = _data.ItemProperties.IPhone664HD;
            return new List<ItemProperty>()
            {
                new ItemProperty() {  ItemVariantId = iphone664gb.ItemVariantId, CharacteristicValueId = 999, OwnerId = iphone664gb.OwnerId },
                new ItemProperty() {  ItemVariantId = iphone664gb.ItemVariantId, CharacteristicValueId = _data.CharacteristicValues.NotSoFashion.Id, OwnerId = iphone664gb.OwnerId },
                new ItemProperty() {  ItemVariantId = 999, CharacteristicValueId = iphone664gb.CharacteristicValueId, OwnerId = iphone664gb.OwnerId },
            };
        }

        protected override Specification<ItemProperty> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<ItemProperty>(i => i == _data.ItemProperties.Samsung732HDHD);
        }

        protected override IEnumerable<ItemProperty> GetCorrectEntitesForUpdate()
        {
            return new List<ItemProperty>() { };
        }

        protected override IEnumerable<ItemProperty> GetIncorrectEntitesForUpdate()
        {
            _data.ItemProperties.Samsung832HDHD.CharacteristicValueId = _data.CharacteristicValues.MuchFashion.Id;
            return new List<ItemProperty>() { _data.ItemProperties.Samsung832HDHD };
        }
        
        protected async Task<IEnumerable<ItemProperty>> GetDuplicateEntitiesAsync()
        {
            var iphone = _data.ItemProperties.IPhone632GB32;
            return await Task.FromResult(new List<ItemProperty>
            {
                new ItemProperty() {  CharacteristicValueId = iphone.CharacteristicValueId, ItemVariantId = iphone.ItemVariantId },
            });
        }
    }
}
