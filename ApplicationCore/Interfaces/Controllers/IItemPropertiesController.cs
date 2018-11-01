using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemPropertiesController: IController<ItemProperty, ItemPropertyViewModel>
    {
        Task<IndexViewModel<ItemPropertyViewModel>> IndexAsync(int? itemVariantId, int? itemId);
        Task<IndexViewModel<ItemPropertyViewModel>> IndexByItemVariantAsync(int itemVariantId);
        Task<IndexViewModel<ItemPropertyViewModel>> IndexByItemAsync(int itemId);
    }
}
