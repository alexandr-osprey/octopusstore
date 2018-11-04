using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class MeasurementUnitServiceTests: ServiceTests<MeasurementUnit, IMeasurementUnitService>
    {
        public MeasurementUnitServiceTests(ITestOutputHelper output):
            base(output)
        {
        }

        protected override IEnumerable<MeasurementUnit> GetCorrectEntitesForUpdate()
        {
            Data.MeasurementUnits.Kg.Title = "Updated";
            return new List<MeasurementUnit>() { Data.MeasurementUnits.Kg };
        }

        protected override IEnumerable<MeasurementUnit> GetCorrectNewEntites()
        {
            return new List<MeasurementUnit>() { new MeasurementUnit() { Title = "Title 1" } };
        }

        protected override Specification<MeasurementUnit> GetEntitiesToDeleteSpecification()
        {
            return new Specification<MeasurementUnit>(m => m.Title == "kg");
        }

        protected override IEnumerable<MeasurementUnit> GetIncorrectEntitesForUpdate()
        {
            Data.MeasurementUnits.Kg.Title = null;
            return new List<MeasurementUnit>() { Data.MeasurementUnits.Kg };
        }

        protected override IEnumerable<MeasurementUnit> GetIncorrectNewEntites()
        {
            return new List<MeasurementUnit>() { new MeasurementUnit() { Title = "" } };
        }
    }
}
