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

        protected override async Task<IEnumerable<MeasurementUnit>> GetCorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<MeasurementUnit>().FirstOrDefaultAsync();
            first.Title = "Updated";
            return new List<MeasurementUnit>()
            {
                first
            };
        }

        protected override async Task<IEnumerable<MeasurementUnit>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<MeasurementUnit>()
            {
                new MeasurementUnit() { Title = "Title 1" }
            });
        }

        protected override Specification<MeasurementUnit> GetEntitiesToDeleteSpecification()
        {
            return new Specification<MeasurementUnit>(m => m.Title == "kg");
        }

        protected override async Task<IEnumerable<MeasurementUnit>> GetIncorrectEntitesForUpdateAsync()
        {
            var first = await _context.Set<MeasurementUnit>().FirstOrDefaultAsync();
            first.Title = null;
            return new List<MeasurementUnit>()
            {
                first
            };
        }

        protected override async Task<IEnumerable<MeasurementUnit>> GetIncorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<MeasurementUnit>()
            {
                new MeasurementUnit() { Title = "" }
            });
        }
    }
}
