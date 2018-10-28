using Xunit.Abstractions;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Services
{
    public class StoreServiceTests : ServiceTests<Store, IStoreService>
    {
        public StoreServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<Store>> GetCorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<Store>().FirstOrDefaultAsync();
            first.Title = "Upd 1";
            first.Description = "Upd 1";
            first.Address = "Upd 1";
            return new List<Store>()
            {
                first
            };
        }

        protected override async Task<IEnumerable<Store>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<Store>()
            {
                new Store() { Title = "New store",  Address = "New address", Description = "New desc "}
            });
        }

        protected override Specification<Store> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Store>(s => s.Title.Contains("Jennifer"));
        }

        protected override async Task<IEnumerable<Store>> GetIncorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<Store>().FirstOrDefaultAsync();
            first.Title = null;
            first.Description = "Upd 1";
            first.Address = "Upd 1";
            return new List<Store>()
            {
                first
            };
        }

        protected override async Task<IEnumerable<Store>> GetIncorrectNewEntitesAsync()
        {
            var first = await _context.Set<Store>().FirstOrDefaultAsync();
            first.Title = null;
            first.Description = "Upd 1";
            first.Address = "Upd 1";
            return new List<Store>()
            {
                //new Store() { Title = first.Title, Description = first.Description, Address = first.Address },
                new Store() { Title = null, Description = first.Description, Address = first.Address },
                new Store() { Title = first.Title, Description = "", Address = first.Address },
                new Store() { Title = first.Title, Description = first.Description, Address = null },
            };
        }
    }
}
