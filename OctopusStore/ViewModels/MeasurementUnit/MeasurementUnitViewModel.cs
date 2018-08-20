using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class MeasurementUnitViewModel : ViewModel<MeasurementUnit>
    {
        public string Title { get; set; }

        public MeasurementUnitViewModel() 
            : base()
        { }
        public MeasurementUnitViewModel(MeasurementUnit measurementUnit)
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
