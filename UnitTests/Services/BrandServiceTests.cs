using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class BrandServiceTests : ServiceTestBase<Brand, IBrandService>
    {
        public BrandServiceTests(ITestOutputHelper output)
            : base(output)
        {
        }
    }
}
