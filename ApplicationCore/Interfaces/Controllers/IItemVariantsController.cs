using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemVariantsController: IController<ItemVariant, ItemVariantViewModel>
    {
        Task<IndexViewModel<ItemVariantViewModel>> IndexAsync(int itemId);
        Task<IndexViewModel<ItemVariantViewModel>> IndexByItemAsync(int itemId);
        Task<ItemVariantDetailViewModel> ReadDetailAsync(int id);
    }
}
