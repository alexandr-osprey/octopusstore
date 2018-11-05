using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class MeasurementUnits: SampleDataEntities<MeasurementUnit>
    {
        public MeasurementUnit M  { get; protected set; }
        public MeasurementUnit Kg  { get; protected set; }
        public MeasurementUnit Pcs  { get; protected set; }

        public MeasurementUnits(StoreContext storeContext): base(storeContext)
        {
            Seed();
            Init();
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

        protected override IQueryable<MeasurementUnit> GetQueryable()
        {
            return base.GetQueryable().Include(m => m.Items);
        }

        public override void Init()
        {
            base.Init();
            M = Entities[0];
            Kg = Entities[1];
            Pcs = Entities[2];
        }
    }
}
