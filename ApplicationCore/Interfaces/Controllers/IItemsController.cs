using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemsController: IController<Item, ItemViewModel>
    {
    }
}
