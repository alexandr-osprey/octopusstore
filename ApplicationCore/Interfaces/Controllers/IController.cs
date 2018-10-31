using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IController<TEntity, TViewModel> where TEntity: Entity where TViewModel: EntityViewModel<TEntity>
    {
        Task<TViewModel> CreateAsync(TViewModel viewModel);
        Task<TViewModel> ReadAsync(int id);
        Task<TViewModel> UpdateAsync(TViewModel viewModel);
        Task<Response> DeleteAsync(int id);
        Task<Response> CheckUpdateAuthorizationAsync(int id);
    }
}
