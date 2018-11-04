using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class BrandsControllerTests: ControllerTests<Brand, BrandViewModel, IBrandsController, IBrandService>
    {
        public BrandsControllerTests(ITestOutputHelper output): base(output)
        {
        }
        
        [Fact]
        public async Task IndexAsync()
        {
            var entities = Data.Brands.Entities;
            var expected = IndexViewModel<BrandViewModel>.FromEnumerableNotPaged(from e in entities select ToViewModel(e));
            var actual = await Controller.IndexAsync();
            Equal(expected, actual);
        }

        protected override IEnumerable<Brand> GetCorrectEntitiesToCreate()
        {
            return new List<Brand>()
            {
                new Brand() { Title = "created"}
            };
        }

        protected override IEnumerable<Brand> GetCorrectEntitiesToUpdate()
        {
            var entities = Data.Brands.Entities;
            entities.ForEach(e => e.Title = "updated");
            return entities;
        }

        protected override BrandViewModel ToViewModel(Brand entity)
        {
            return new BrandViewModel()
            {
                Id = entity.Id,
                Title = entity.Title
            };
        }
    }
}
