using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemsController: IController<Item, ItemViewModel>
    {
        Task<ItemDetailViewModel> ReadDetailAsync(int id);
        Task<IndexViewModel<ItemThumbnailViewModel>> IndexThumbnailsAsync(
            int? page, int? pageSize, string searchValue, int? categoryId, int? storeId, int? brandId, string orderBy, string characteristicsFilter, bool? orderByDescending);
        Task<IndexViewModel<ItemViewModel>> IndexAsync(
            int? page, int? pageSize, string searchValue, int? categoryId, int? storeId, int? brandId, string orderBy, string characteristicsFilter, bool? orderByDescending);
    }
}
