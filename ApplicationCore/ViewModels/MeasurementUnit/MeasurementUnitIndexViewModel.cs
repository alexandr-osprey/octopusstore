using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class MeasurementUnitIndexViewModel : EntityIndexViewModel<MeasurementUnitViewModel, MeasurementUnit>
    {
        public MeasurementUnitIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<MeasurementUnit> measurementUnits)
            : base(page, totalPages, totalCount, from m in measurementUnits select new MeasurementUnitViewModel(m))
        {
        }
    }
}
