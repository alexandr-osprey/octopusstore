using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICharacteristicsController: IController<Characteristic, CharacteristicViewModel>
    {
        Task<IndexViewModel<CharacteristicViewModel>> IndexAsync(int categoryId);
    }
}
