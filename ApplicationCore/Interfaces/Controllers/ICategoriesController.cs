using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICategoriesController: IController<Category, CategoryViewModel>
    {
        Task<IndexViewModel<CategoryViewModel>> Index(int? categoryId, int? storeId);
    }
}
