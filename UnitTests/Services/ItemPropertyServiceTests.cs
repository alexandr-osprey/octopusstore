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
    public class ItemPropertyServiceTests : ServiceTests<ItemProperty, IItemPropertyService>
    {
        public ItemPropertyServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<ItemProperty>> GetCorrectNewEntitesAsync()
        {
            var newVariant = await _context.Set<ItemVariant>().AddAsync(new ItemVariant() { ItemId = 1, OwnerId = johnId, Price = 100 });
            await _context.SaveChangesAsync();
            return new List<ItemProperty>()
                {
                    new ItemProperty() { ItemVariantId = newVariant.Entity.Id, CharacteristicValueId = 1},
                    new ItemProperty() { ItemVariantId = newVariant.Entity.Id, CharacteristicValueId = 4 },
                };
        }
        protected override async Task<IEnumerable<ItemProperty>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<ItemProperty>()
                {
                    new ItemProperty() {  ItemVariantId = 1, CharacteristicValueId = 999, OwnerId = johnId },
                    new ItemProperty() {  ItemVariantId = 1, CharacteristicValueId = 8, OwnerId = johnId },
                    new ItemProperty() {  ItemVariantId = 999, CharacteristicValueId = 5, OwnerId = johnId },
                });
        }
        protected override Specification<ItemProperty> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<ItemProperty>(2);
        }
        protected override async Task<IEnumerable<ItemProperty>> GetCorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<ItemProperty>().FirstAsync();
            first.CharacteristicValueId = 1;
            return new List<ItemProperty>() { first };
        }
        protected override async Task<IEnumerable<ItemProperty>> GetIncorrectEntitesForUpdateAsync()
        {
            return await Task.FromResult(
                new List<ItemProperty>()
                {
                 //   new Characteristic() { Id = first.Id, Title = first.Title, CategoryId = first.CategoryId, OwnerId = first.OwnerId },
                });
        }
        [Fact]
        public async Task CreateAsyncThrowsAlreadyExists()
        {
            foreach (var duplicateEntity in await GetDuplicateEntitiesAsync())
            {
                await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.CreateAsync(duplicateEntity));
            }
        }
        protected async Task<IEnumerable<ItemProperty>> GetDuplicateEntitiesAsync()
        {
            return await Task.FromResult(new List<ItemProperty>
            {
                // first two already exist
                new ItemProperty() {  CharacteristicValueId = 2, ItemVariantId = 1 },
                new ItemProperty() {  CharacteristicValueId = 1, ItemVariantId = 1 },
                new ItemProperty() { CharacteristicValueId = 4, ItemVariantId = 5 },
            });
        }
    }
}
