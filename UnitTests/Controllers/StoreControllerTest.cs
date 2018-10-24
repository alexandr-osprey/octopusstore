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

namespace UnitTests.Controllers
{
    public class StoreControllerTest : ControllerTestBase<Store, StoresController, IStoreService>
    {
        public StoreControllerTest(ITestOutputHelper output)
            : base(output)
        { }

        [Fact]
        public async Task Index()
        {
            int page = 2;
            int pageSize = 1;
            var actual = await controller.Index(2, 1, null, null);
            var stores = await GetQueryable(context)
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .ToListAsync();
            int totalCount = context.Stores.Count();
            var expected = new IndexViewModel<StoreViewModel>(2, GetPageCount(totalCount, pageSize), totalCount, from s in stores select new StoreViewModel(s));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Get()
        {
            var store = await GetQueryable(context).FirstOrDefaultAsync();
            var expected = new StoreViewModel(store);
            var actual = await controller.Get(store.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task GetDetail()
        {
            var store = await GetQueryable(context).FirstOrDefaultAsync();
            var expected = new StoreViewModel(store);
            var actual = await controller.GetDetail(store.Id);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Post()
        {
            var store = await GetQueryable(context).FirstOrDefaultAsync();
            store.Title += "_ADDED";
            store.Id = 0;
            store.Address = "Some address";
            store.Address = "new description";
            var storeViewModel = new StoreViewModel(store);
            var actual = await controller.Post(storeViewModel);
            var expected = new StoreViewModel(
                await GetQueryable(context).FirstOrDefaultAsync(s => s.Id == actual.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Put()
        {
            var store = await GetQueryable(context).FirstOrDefaultAsync();
            store.Title += "_UPDATED";
            var storeViewModel = new StoreViewModel(store);
            var actual = await controller.Put(storeViewModel.Id, storeViewModel);
            var expected = new StoreViewModel(
                await GetQueryable(context).FirstOrDefaultAsync(s => s.Id == actual.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Delete()
        {
            var store = await GetQueryable(context).FirstOrDefaultAsync();
            await controller.Delete(store.Id);
            Assert.False(context.Stores.Where(i => i.Id == store.Id).Any());
        }
        protected override IQueryable<Store> GetQueryable(DbContext context)
        {
            return context
                .Set<Store>()
                .AsNoTracking()
                .Include(s => s.Items);
        }
    }
}
