using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class BrandServiceTests : ServiceTests<Brand, IBrandService>
    {
        public BrandServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<Brand> GetCorrectNewEntites()
        {
            return new List<Brand>() { new Brand() { Title = "Brand title 1"}, new Brand() { Title = "Brand title 2"} };
        }

        protected override IEnumerable<Brand> GetIncorrectNewEntites()
        {
            return new List<Brand>() { new Brand() { Title = null} };
        }

        protected override Specification<Brand> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Brand>(b => b.Title.Contains("Samsung"));
        }

        protected override IEnumerable<Brand> GetCorrectEntitesForUpdate()
        {
            Data.Brands.Apple.Title = "Updated";
            return new List<Brand>() { Data.Brands.Apple };
        }

        protected override IEnumerable<Brand> GetIncorrectEntitesForUpdate()
        {
            Data.Brands.UnitedColorsOfBenetton.Title = null;
            Data.Brands.Apple.Title = "";
            Data.Brands.CalvinKlein.Title = " ";
            return new List<Brand>()
            {
                Data.Brands.UnitedColorsOfBenetton,
                Data.Brands.Apple,
                Data.Brands.CalvinKlein
            };
        }

        [Fact]
        public async Task DeleteSingleWithRelatedRelinkAsync()
        {
            var brand = Data.Brands.Apple;
            int idToRelinkTo = Data.Brands.Pebble.Id;
            var items = Data.Items.Entities.Where(i => i.Brand == brand).ToList();
            await Service.DeleteSingleWithRelatedRelink(brand.Id, idToRelinkTo);
            items.ForEach(i => Assert.Equal(i.BrandId, idToRelinkTo));
            Assert.False(Context.Set<Brand>().Any(b => b == brand));
        }
    }
}
