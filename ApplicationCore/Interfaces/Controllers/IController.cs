using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IController<TEntity, TViewModel> where TEntity: Entity where TViewModel: EntityViewModel<TEntity>
    {
        Task<TViewModel> Create(TViewModel viewModel);
        Task<TViewModel> Read(int id);
        Task<TViewModel> Update(TViewModel viewModel);
        Task<TViewModel> Delete(int id);
        Task<IndexViewModel<TViewModel>> Index();
        Task<Response> CheckUpdateAuthorization(int id);
    }
}
