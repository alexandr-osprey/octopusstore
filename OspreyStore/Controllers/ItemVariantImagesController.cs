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
using ApplicationCore.Identity;

namespace OspreyStore.Controllers
{
    [Route("api/[controller]")]
    public class ItemVariantImagesController: CRUDController<IItemVariantImageService, ItemVariantImage, ItemVariantImageViewModel>, IItemVariantImagesController
    {
        public ItemVariantImagesController(
            IItemVariantImageService service,
            IActivatorService activatorService,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<ItemVariantImage, ItemVariantImageViewModel>> logger)
           : base(service, activatorService, identityService, scopedParameters, logger)
        {
        }

        [HttpPost("/api/itemVariants/{relatedId:int}/image")]
        public async Task<ItemVariantImageViewModel> PostFormAsync(int relatedId, [FromForm]IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    await formFile.CopyToAsync(stream);
                    var itemVariantImage = new ItemVariantImage(formFile.FileName, formFile.ContentType, relatedId, null, stream);
                    return await GetViewModelAsync<ItemVariantImageViewModel>(await _service.CreateAsync(itemVariantImage));
                }
                catch (Exception exception)
                {
                    string message = $"Error saving item image: {exception}";
                    _logger.Warn(exception, message);
                    throw new Exception(message);
                }
            }
        }

        [HttpPut]
        public override async Task<ItemVariantImageViewModel> UpdateAsync([FromBody]ItemVariantImageViewModel itemVariantImageViewModel) => await base.UpdateAsync(itemVariantImageViewModel);

        [AllowAnonymous]
        [HttpGet]
        [HttpGet("/api/itemVariants/{itemVariantId}/images")]
        public async Task<IndexViewModel<ItemVariantImageViewModel>> IndexAsync([FromQuery(Name = "itemVariantId")]int itemVariantId)
        {
            var spec = new Specification<ItemVariantImage>((i => i.RelatedId == itemVariantId))
            {
                Description = $"ItemVariantImage with RelatedId={itemVariantId}"
            };
            return await base.IndexNotPagedAsync(spec);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}/file")]
        public async Task<FileStreamResult> GetFileAsync(int id)
        {
            if (id == 0)
                return null;
            try
            {
                var image = await _service.ReadSingleAsync(new EntitySpecification<ItemVariantImage>(id));
                return new FileStreamResult(_service.GetStream(image), image.ContentType);
            }
            catch (Exception exception)
            {
                string message = $"Error retrieving item variant image id = {id}.";
                _logger.Warn(exception, message);
                throw new Exception(message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<ItemVariantImageViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id) => await base.DeleteSingleAsync(new EntitySpecification<ItemVariantImage>(id));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}