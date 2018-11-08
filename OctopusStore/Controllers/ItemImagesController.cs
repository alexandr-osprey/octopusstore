using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Route("api/[controller]")]
    public class ItemImagesController: CRUDController<IItemImageService, ItemImage, ItemImageViewModel>, IItemImagesController
    {
        public ItemImagesController(
            IItemImageService service,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<ItemImage, ItemImageViewModel>> logger)
           : base(service, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost("/api/items/{relatedId:int}/itemImage")]
        public async Task<ItemImageViewModel> PostFormAsync(int relatedId, [FromForm]IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    await formFile.CopyToAsync(stream);
                    var itemImage = new ItemImage(formFile.FileName, formFile.ContentType, relatedId, stream);
                    return GetViewModel<ItemImageViewModel>(await Service.CreateAsync(itemImage));
                }
                catch (Exception exception)
                {
                    string message = $"Error saving item image.";
                    Logger.Warn(exception, message);
                    throw new Exception(message);
                }
            }
        }

        [HttpPut]
        public override async Task<ItemImageViewModel> UpdateAsync([FromBody]ItemImageViewModel itemImageViewModel) => await base.UpdateAsync(itemImageViewModel);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<ItemImageViewModel>> IndexAsync([FromQuery(Name = "itemId")]int itemId)
        {
            var spec = new Specification<ItemImage>((i => i.RelatedId == itemId))
            {
                Description = $"ItemImage with RelatedId={itemId}"
            };
            return await base.IndexNotPagedAsync(spec);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}/file")]
        public async Task<FileStreamResult> GetFileAsync(int id)
        {
            try
            {
                var image = await Service.ReadSingleAsync(new EntitySpecification<ItemImage>(id));
                return new FileStreamResult(Service.GetStream(image), image.ContentType);
            }
            catch (Exception exception)
            {
                string message = $"Error retrieving item image id = {id}.";
                Logger.Warn(exception, message);
                throw new Exception(message);
            }
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<ItemImageViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteSingleAsync(new EntitySpecification<ItemImage>(id));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}