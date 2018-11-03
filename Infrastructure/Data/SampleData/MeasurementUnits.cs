using ApplicationCore.Entities;
using System.Collections.Generic;

namespace Infrastructure.Data.SampleData
{
    public class MeasurementUnits: SampleDataEntities<MeasurementUnit>
    {
        public MeasurementUnit M { get; }
        public MeasurementUnit Kg { get; }
        public MeasurementUnit Pcs { get; }

        public MeasurementUnits(StoreContext storeContext): base(storeContext)
        {
            Seed();

            M = Entities[0];
            Kg = Entities[1];
            Pcs = Entities[2];
        }

        protected override IEnumerable<MeasurementUnit> GetSourceEntities()
        {
            return new List<MeasurementUnit>()
            {
                new MeasurementUnit { Title = "m", OwnerId = Users.AdminId },
                new MeasurementUnit { Title = "kg", OwnerId = Users.AdminId },
                new MeasurementUnit { Title = "pcs", OwnerId = Users.AdminId }
            };
        }
    }
}
