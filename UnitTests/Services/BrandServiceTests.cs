using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class BrandServiceTests : ServiceTests<Brand, IBrandService>
    {
        public BrandServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<Brand>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<Brand>()
                {
                    new Brand() { Title = "Brand title 1"},
                    new Brand() { Title = "Brand title 2"},
                });
        }
        protected override async Task<IEnumerable<Brand>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(
                new List<Brand>()
                {
                    new Brand() { Title = null},
                });
        }
        protected override Specification<Brand> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Brand>(b => b.Title.Contains("Samsung"));
        }
        protected override async Task<IEnumerable<Brand>> GetCorrectEntitesForUpdateAsync()
        {
            var apple = await GetQueryable().FirstOrDefaultAsync(b => b.Title == "Apple");
            apple.Title = "Apple 1";
            return new List<Brand>() { apple };
        }
        protected override async Task<IEnumerable<Brand>> GetIncorrectEntitesForUpdateAsync()
        {
            return await Task.FromResult(
                new List<Brand>()
                {
                    new Brand() { Id = 1, Title = null},
                    new Brand() { Id = 1, Title = "" },
                    new Brand() { Id = 1, Title = " "},
                });
        }
    }
}
