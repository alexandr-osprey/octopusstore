using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IStoresController: IController<Store, StoreViewModel>
    {
        Task<IndexViewModel<StoreViewModel>> IndexAsync(int? page, int? pageSize, bool? updateAuthorizationFilter, string title);
    }
}
