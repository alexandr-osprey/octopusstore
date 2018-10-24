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

namespace OctopusStore.Controllers
{
    [Route("api/[controller]")]
    public class ItemImagesController 
        : CRUDController<
            IItemImageService, 
            ItemImage, 
            ItemImageViewModel, 
            ItemImageDetailViewModel, 
            ItemImageIndexViewModel>
    {
        public ItemImagesController(
            IItemImageService service, 
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<ItemImage>> logger)
            : base(service, scopedParameters, logger)
        {
        }

        [HttpPost("/api/items/{relatedId:int}/itemImage")]
        public async Task<ItemImageViewModel> PostForm(int relatedId, [FromForm]IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    await formFile.CopyToAsync(stream);
                    var itemImage = new ItemImage(formFile.FileName, this.User.Identity.Name, formFile.ContentType, relatedId, stream);
                    return GetViewModel<ItemImageViewModel>(await _service.CreateAsync(itemImage));
                }
                catch (Exception exception)
                {
                    string message = $"Error saving item image.";
                    _logger.Warn(exception, message);
                    throw new Exception(message);
                }
            }
        }
        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id)
        {
            return await base.CheckUpdateAuthorizationAsync(id);
        }
        [HttpPut("{id:int}")]
        public async Task<ItemImageViewModel> Put(int id, [FromBody]ItemImageViewModel itemImageViewModel)
        {
            itemImageViewModel.Id = id;
            return await base.UpdateAsync(itemImageViewModel);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ItemImageIndexViewModel> Index([FromQuery(Name = "itemId")]int itemId)
        {
            var spec = new Specification<ItemImage>((i => i.RelatedId == itemId))
            {
                Description = $"ItemImage with RelatedId={itemId}"
            };
            return await base.IndexNotPagedAsync(spec);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}/file")]
        public async Task<ActionResult> GetFile(int id)
        {
            try
            {
                var image = await _service.ReadSingleAsync(new EntitySpecification<ItemImage>(id));
                return new FileStreamResult(_service.GetStream(image), image.ContentType);
            }
            catch (Exception exception)
            {
                string message = $"Error retrieving item image id = {id}.";
                _logger.Warn(exception, message);
                throw new Exception(message);
            }
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ItemImageViewModel> Get(int id)
        {
            return await base.GetAsync(new EntitySpecification<ItemImage>(id));
        }
        [AllowAnonymous]
        [HttpGet("{id:int}/details")]
        public async Task<ItemImageDetailViewModel> GetDetal(int id)
        {
            return await base.GetDetailAsync(new ItemImageDetailSpecification(id));
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteSingleAsync(new ItemImageDetailSpecification(id));
        }
    }
}