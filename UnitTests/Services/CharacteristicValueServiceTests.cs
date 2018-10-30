using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CharacteristicValueServiceTests: ServiceTests<CharacteristicValue, ICharacteristicValueService>
    {
        public CharacteristicValueServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<CharacteristicValue>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<CharacteristicValue>()
                {
                    new CharacteristicValue() { Title = "title 1",  CharacteristicId = 1 },
                    new CharacteristicValue() { Title = "title 2", CharacteristicId = 3 },
                });
        }
        protected override async Task<IEnumerable<CharacteristicValue>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<CharacteristicValue>()
                {
                    new CharacteristicValue() { Title = null, CharacteristicId = 2 },
                    new CharacteristicValue() { Title = "new2", CharacteristicId = 0 },
                });
        }
        protected override Specification<CharacteristicValue> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<CharacteristicValue>(2);
        }
        protected override async Task<IEnumerable<CharacteristicValue>> GetCorrectEntitesForUpdateAsync()
        {
            var storage = await _context.Set<CharacteristicValue>().FirstOrDefaultAsync(b => b.Title.Contains("32"));
            storage.Title = "Updated storage";
            return new List<CharacteristicValue>() { storage };
        }
        protected override async Task<IEnumerable<CharacteristicValue>> GetIncorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<CharacteristicValue>().FirstAsync();
            first.Title = null;
            return await Task.FromResult(
                new List<CharacteristicValue>()
                {
                 //   new Characteristic() { Id = first.Id, Title = first.Title, CategoryId = first.CategoryId, OwnerId = first.OwnerId },
                    first
                });
        }
    }
}
