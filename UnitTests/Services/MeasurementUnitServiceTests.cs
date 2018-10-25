using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class MeasurementUnitServiceTests: ServiceTestBase<MeasurementUnit, IMeasurementUnitService>
    {
        public MeasurementUnitServiceTests(ITestOutputHelper output) :
            base(output)
        { }
    }
}
