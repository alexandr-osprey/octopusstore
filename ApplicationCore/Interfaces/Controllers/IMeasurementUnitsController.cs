using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IMeasurementUnitsController: IController<MeasurementUnit, MeasurementUnitViewModel>
    {
        Task<IndexViewModel<MeasurementUnitViewModel>> IndexAsync();
    }
}
