using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Controllers
{
    public interface IItemImagesController: IController<ItemImage, ItemImageViewModel>
    {
        Task<ItemImageViewModel> PostFormAsync(int relatedId, IFormFile formFile);
        Task<FileStreamResult> GetFileAsync(int id);
        Task<IndexViewModel<ItemImageViewModel>> IndexAsync(int itemId);
    }
}
