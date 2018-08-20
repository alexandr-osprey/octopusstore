using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using OctopusStore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class MeasurementUnitControllerTests : ControllerTestBase<MeasurementUnit, MeasurementUnitsController, IMeasurementUnitService>
    {
        public MeasurementUnitControllerTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task Index()
        {
            var actual = await controller.Index();
            var measurementUnits = await GetQueryable(context)
                .ToListAsync();

            var expected = new MeasurementUnitIndexViewModel(1, 1, measurementUnits.Count(), measurementUnits);
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
    }
}
