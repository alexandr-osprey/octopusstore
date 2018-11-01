using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICharacteristicValuesController: IController<CharacteristicValue, CharacteristicValueViewModel>
    {
        Task<IndexViewModel<CharacteristicValueViewModel>> IndexAsync(int categoryId);
    }
}
