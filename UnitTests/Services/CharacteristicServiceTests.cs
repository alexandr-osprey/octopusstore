using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CharacteristicServiceTests : ServiceTests<Characteristic, ICharacteristicService>
    {
        public CharacteristicServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<Characteristic>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<Characteristic>()
                {
                    new Characteristic() { Title = "title 1", CategoryId = 2 },
                    new Characteristic() { Title = "title 2", CategoryId = 3},
                });
        }
        protected override async Task<IEnumerable<Characteristic>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<Characteristic>()
                {
                    new Characteristic() { Title = null, CategoryId = 2 },
                    new Characteristic() { Title = "new2", CategoryId = 0 },
                });
        }
        protected override Specification<Characteristic> GetEntitiesToDeleteSpecification()
        {
            return new EntitySpecification<Characteristic>(2);
        }
        protected override async Task<IEnumerable<Characteristic>> GetCorrectEntitesForUpdateAsync()
        {
            var storage = await _context.Set<Characteristic>().FirstOrDefaultAsync(b => b.Title == "Storage");
            storage.Title = "Updated storage";
            return new List<Characteristic>() { storage };
        }
        protected override async Task<IEnumerable<Characteristic>> GetIncorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<Characteristic>().FirstAsync();
            return await Task.FromResult(
                new List<Characteristic>()
                {
                 //   new Characteristic() { Id = first.Id, Title = first.Title, CategoryId = first.CategoryId, OwnerId = first.OwnerId },
                    new Characteristic() { Id = first.Id, Title = "", CategoryId = first.CategoryId, OwnerId = first.OwnerId },
                });
        }
    }
}
