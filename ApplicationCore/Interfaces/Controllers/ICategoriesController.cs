using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICategoriesController: IController<Category, CategoryViewModel>
    {
        Task<CategoryViewModel> ReadRootAsync();
        Task<IndexViewModel<CategoryViewModel>> IndexAsync(int? categoryId, int? storeId);
    }
}
