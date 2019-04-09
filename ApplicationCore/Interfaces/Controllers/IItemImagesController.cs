using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemVariantImagesController: IController<ItemVariantImage, ItemVariantImageViewModel>
    {
        Task<ItemVariantImageViewModel> PostFormAsync(int relatedId, IFormFile formFile);
        Task<FileStreamResult> GetFileAsync(int id);
        Task<IndexViewModel<ItemVariantImageViewModel>> IndexAsync(int itemId);
    }
}
