using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class MeasurementUnitViewModel: EntityViewModel<MeasurementUnit>
    {
        public string Title { get; set; }

        public MeasurementUnitViewModel(): base()
        {
        }
        public MeasurementUnitViewModel(MeasurementUnit measurementUnit): base(measurementUnit)
        {
            Title = measurementUnit.Title;
        }

        public override MeasurementUnit ToModel()
        {
            return new MeasurementUnit()
            {
                Id = Id,
                Title = Title
            };
        }
        public override MeasurementUnit UpdateModel(MeasurementUnit modelToUpdate)
        {
            modelToUpdate.Title = Title;
            return modelToUpdate;
        }
    }
}
