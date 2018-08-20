using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class MeasurementUnitDetailViewModel : DetailViewModel<MeasurementUnit>
    {
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
        public override void UpdateModel(MeasurementUnit modelToUpdate)
        {
            modelToUpdate.Title = Title;
        }
    }
}
