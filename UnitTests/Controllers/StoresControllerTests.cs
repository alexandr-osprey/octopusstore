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
using Infrastructure.Data.SampleData;

namespace UnitTests.Controllers
{
    public class StoresControllerTests : ControllerTests<Store, StoreViewModel, IStoresController, IStoreService>
    {
        public StoresControllerTests(ITestOutputHelper output)
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
            return Context.Set<Store>().Include(s => s.Items);
        }

        protected override IEnumerable<Store> GetCorrectEntitiesToCreate()
        {
            Controller.ScopedParameters.ClaimsPrincipal = Users.AdminPrincipal;
            return new List<Store>() { new Store() { Title = "Store 1", Address = "Address", Description = "Desc", OwnerId = Users.AdminId } };
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

        protected override IEnumerable<StoreViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<StoreViewModel>()
            {
                new StoreViewModel()
                {
                    Id = Data.Stores.Johns.Id,
                    Title = "Updated",
                    Description = "Description",
                    Address = "Address",
                    OwnerId = "incorrect owner",
                }
            };
        }

        protected override Task AssertUpdateSuccessAsync(Store beforeUpdate, StoreViewModel expected, StoreViewModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Address, actual.Address);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(beforeUpdate.OwnerId, actual.OwnerId);
            return Task.CompletedTask;
        }
    }
}
