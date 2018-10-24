using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class MeasurementUnitDetailViewModel : EntityDetailViewModel<MeasurementUnit>
    {
        public string Title { get; set; }

        public MeasurementUnitDetailViewModel(MeasurementUnit measurementUnit)
            : base(measurementUnit)
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
