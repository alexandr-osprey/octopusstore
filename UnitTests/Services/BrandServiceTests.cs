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
            _data.Brands.Apple.Title = "Updated";
            return new List<Brand>() { _data.Brands.Apple };
        }

        protected override IEnumerable<Brand> GetIncorrectEntitesForUpdate()
        {
            _data.Brands.UnitedColorsOfBenetton.Title = null;
            _data.Brands.Apple.Title = "";
            _data.Brands.DanielePatrici.Title = " ";
            return new List<Brand>()
            {
                _data.Brands.UnitedColorsOfBenetton,
                _data.Brands.Apple,
                _data.Brands.DanielePatrici
            };
        }
    }
}
