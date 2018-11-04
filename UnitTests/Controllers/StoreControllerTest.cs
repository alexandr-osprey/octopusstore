using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;
using Infrastructure.Data.SampleData;

namespace UnitTests.Controllers
{
    public class StoreControllerTest : ControllerTests<Store, StoreViewModel, IStoresController, IStoreService>
    {
        public StoreControllerTest(ITestOutputHelper output)
           : base(output)
        {
        }

        [Fact]
        public async Task IndexAsync()
        {
            int page = 2;
            int pageSize = 1;
            var actual = await Controller.IndexAsync(page, pageSize, null, null);
            var stores = await GetQueryable()
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            int totalCount = Data.Stores.Entities.Count;
            var expected = new IndexViewModel<StoreViewModel>(page, GetPageCount(totalCount, pageSize), totalCount, from s in stores select new StoreViewModel(s));
            Equal(expected, actual);
        }


        protected override IQueryable<Store> GetQueryable()
        {
            return Context
                .Set<Store>()
                .AsNoTracking()
                .Include(s => s.Items);
        }

        protected override IEnumerable<Store> GetCorrectEntitiesToCreate()
        {
            return new List<Store>() { new Store() { Title = "Store 1", Address = "Address", Description = "Desc", OwnerId = Users.JohnId } };
        }

        protected override StoreViewModel ToViewModel(Store entity)
        {
            return new StoreViewModel()
            {
                Title = entity.Title,
                Address = entity.Address,
                Description = entity.Description,
                Id = entity.Id,
                OwnerId = entity.OwnerId,
                RegistrationDate = entity.RegistrationDate
            };
        }

        protected override IEnumerable<Store> GetCorrectEntitiesToUpdate()
        {
            var store = Data.Stores.Jennifers;
            store.Title = "Updated";
            store.Description = "Description";
            store.Address = "Address";
            store.OwnerId = "incorrect owner";
            return new List<Store>() { store };
        }

        protected override void AssertUpdateSuccess(Store beforeUpdate, StoreViewModel expected, StoreViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Address, actual.Address);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(beforeUpdate.OwnerId, actual.OwnerId);
        }
    }
}
