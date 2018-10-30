using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface ICategoriesController: IController<Category, CategoryViewModel>
    {
    }
}
