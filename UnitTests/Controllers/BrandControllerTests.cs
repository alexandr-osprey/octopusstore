using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using OctopusStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class BrandControllerTests : ControllerTestBase<Brand, BrandController, IBrandService>
    {
        public BrandControllerTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task Index()
        {
            var actual = await controller.Index();
            var brands = await GetQueryable(context).ToListAsync();
            var expected = new BrandIndexViewModel(1, 1, brands.Count(), brands);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        [Fact]
        public async Task Get()
        {
            var first = GetQueryable(context).FirstOrDefaultAsync();
            var actual = await controller.Get(first.Id);
            var brand = await GetQueryable(context)
                .FirstOrDefaultAsync(b => b.Id == first.Id);
            var expected = new BrandViewModel(brand);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
        [Fact]
        public async Task GetDetail()
        {
            var first = GetQueryable(context).FirstOrDefaultAsync();
            var actual = await controller.GetDetail(first.Id);
            var brand = await GetQueryable(context)
                .FirstOrDefaultAsync(b => b.Id == first.Id);
            var expected = new BrandDetailViewModel(brand);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
    }
}
