using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
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
    public class BrandControllerTests : ControllerTestBase<Brand, BrandsController, IBrandService>
    {
        public BrandControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Index()
        {
            var actual = await controller.Index();
            var brands = await GetQueryable(context).Where(b => true).ToListAsync();
            var expected = new IndexViewModel<BrandViewModel>(1, 1, brands.Count(), from b in brands select new BrandViewModel(b));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Get()
        {
            Brand first = await GetQueryable(context).FirstOrDefaultAsync();
            BrandViewModel actual = await controller.Get(first.Id);
            Brand brand = await GetQueryable(context)
                .FirstOrDefaultAsync(b => b.Id == first.Id);
            BrandViewModel expected = new BrandViewModel(brand);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
    }
}
