using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class ItemVariantServiceTests : ServiceTests<ItemVariant, IItemVariantService>
    {
        public ItemVariantServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<ItemVariant>> GetCorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<ItemVariant>().FirstOrDefaultAsync();
            first.Title = "Updated";
            first.Price = 999;
            return new List<ItemVariant>()
            {
                first
            };
        }

        protected override async Task<IEnumerable<ItemVariant>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<ItemVariant>()
            {
                new ItemVariant() { ItemId = 1, Title = "title 1", Price = 500 },
                new ItemVariant() { ItemId = 2, Title = "title 2", Price = 100 },
                new ItemVariant() { ItemId = 3, Title = "title 3", Price = 300 },
            });
        }

        protected override Specification<ItemVariant> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemVariant>(i => i.Title.Contains("iPhone"));
        }

        protected override async Task<IEnumerable<ItemVariant>> GetIncorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<ItemVariant>().FirstOrDefaultAsync();
            return new List<ItemVariant>()
            {
                new ItemVariant() { Id = first.Id, Title = "", ItemId = first.ItemId, OwnerId = first.OwnerId, Price = first.Price },
                new ItemVariant() { Id = first.Id, Title = first.Title, ItemId = first.ItemId, OwnerId = first.OwnerId, Price = 0 },
            };
        }

        protected override async Task<IEnumerable<ItemVariant>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<ItemVariant>()
            {
                new ItemVariant() { ItemId = 0, Title = "title 1", Price = 500 },
                new ItemVariant() { ItemId = 2, Title = null, Price = 100 },
                new ItemVariant() { ItemId = 3, Title = "title 3", Price = 0 },
            });
        }

        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.ItemVariantCharacteristicValues);
        }
    }
}
