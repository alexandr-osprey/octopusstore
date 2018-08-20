using Xunit.Abstractions;
using ApplicationCore.Interfaces;
using ApplicationCore.Entities;

namespace UnitTests.Services
{
    public class StoreServiceTests : ServiceTestBase<Store, IStoreService>
    {
        public StoreServiceTests(ITestOutputHelper output)
            : base(output)
        { }
    }
}
