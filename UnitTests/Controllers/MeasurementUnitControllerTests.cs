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
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    public class MeasurementUnitControllerTests : ControllerTests<MeasurementUnit, MeasurementUnitViewModel, IMeasurementUnitsController, IMeasurementUnitService>
    {
        public MeasurementUnitControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Index()
        {
            var actual = await Controller.IndexAsync();
            var measurementUnits = Data.MeasurementUnits.Entities;
            var expected = new IndexViewModel<MeasurementUnitViewModel>(1, 1, measurementUnits.Count(), from m in measurementUnits select new MeasurementUnitViewModel(m));
            Equal(expected, actual);
        }

        protected override IEnumerable<MeasurementUnit> GetCorrectEntitiesToCreate()
        {
            return new List<MeasurementUnit>() { new MeasurementUnit() { Title = "new" } };
        }

        protected override IEnumerable<MeasurementUnit> GetCorrectEntitiesToUpdate()
        {
            Data.MeasurementUnits.Kg.Title = "KGB";
            return new List<MeasurementUnit>() { Data.MeasurementUnits.Kg };
        }

        protected override MeasurementUnitViewModel ToViewModel(MeasurementUnit entity)
        {
            return new MeasurementUnitViewModel()
            {
                Id = entity.Id,
                Title = entity.Title
            };
        }
    }
}
