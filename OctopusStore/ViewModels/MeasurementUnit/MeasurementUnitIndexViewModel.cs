using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class MeasurementUnitIndexViewModel : IndexViewModel<MeasurementUnitViewModel, MeasurementUnit>
    {
        public MeasurementUnitIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<MeasurementUnit> measurementUnits)
            : base(page, totalPages, totalCount, from m in measurementUnits select new MeasurementUnitViewModel(m))
        { }
    }
}
