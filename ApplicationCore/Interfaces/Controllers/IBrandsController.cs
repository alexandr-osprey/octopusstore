using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IBrandsController: IController<Brand, BrandViewModel>
    {
        Task<IndexViewModel<BrandViewModel>> IndexAsync();
    }
}
