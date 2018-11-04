using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IStoresController: IController<Store, StoreViewModel>
    {
        Task<IndexViewModel<StoreViewModel>> IndexAsync(int? page, int? pageSize, bool? updateAuthorizationFilter, string title);
        Task<Response> CreateStoreAdministratorAsync(int storeId, string email);
        Task<IndexViewModel<string>> GetStoreAdministratorsAsync(int storeId);
        Task<Response> DeleteStoreAdministratorAsync(int storeId, string email);
    }
}
