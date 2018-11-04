using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Unit to measure Item
    /// </summary>
    public class MeasurementUnit: Entity, IGenericMemberwiseClonable<MeasurementUnit>
    {
        public string Title { get; set; }

        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public MeasurementUnit(): base()
        {
        }

        protected MeasurementUnit(MeasurementUnit measurementUnit): base(measurementUnit)
        {
            Title = measurementUnit.Title;
        }

        public MeasurementUnit ShallowClone()
        {
            return (MeasurementUnit)MemberwiseClone();
        }
    }
}
