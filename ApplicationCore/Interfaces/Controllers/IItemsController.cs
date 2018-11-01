using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemsController: IController<Item, ItemViewModel>
    {
        Task<IndexViewModel<ItemThumbnailViewModel>> IndexThumbnailsAsync(int? page, int? pageSize, string title, int? categoryId, int? storeId, int? brandId);
        Task<IndexViewModel<ItemViewModel>> IndexAsync(int? page, int? pageSize, string title, int? categoryId, int? storeId, int? brandId);
        Task<ItemDetailViewModel> ReadDetailAsync(int id);
    }
}
