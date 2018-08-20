using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OctopusStore.Controllers
{
    [Route("api/[controller]")]
    public class ItemImagesController 
        : ReadWriteController<
            IItemImageService, 
            ItemImage, 
            ItemImageViewModel, 
            ItemImageDetailViewModel, 
            ItemImageIndexViewModel>
    {
        private readonly IItemService _itemService;
        private static readonly IEnumerable<string> allowedContentTypes = new List<string>() { @"image/jpeg", @"image/png" };

        public ItemImagesController(IItemImageService itemImageService, IItemService itemService, IAppLogger<IEntityController<ItemImage>> logger)
            : base(itemImageService, logger)
        {
            _itemService = itemService;
        }

        [HttpPost("file")]
        public async Task<ItemImageViewModel> PostForm([FromForm]IFormFile formFile)
        {
            if (ValidateImageFile(formFile))
            { }
            var imageFile = formFile;
            int relatedId = 0;
            Int32.TryParse(imageFile.FileName, out relatedId);
            string ownerUsername = "john@mail.com";
            using (var stream = new MemoryStream())
            {
                try
                {
                    await imageFile.CopyToAsync(stream);
                    var itemImage = new ItemImage(ownerUsername, imageFile.ContentType, relatedId, stream);
                    await _serivce.AddAsync(itemImage);
                    var result = new ItemImageViewModel(itemImage);
                    return result;
                }
                catch (Exception exception)
                {
                    string message = $"Error saving item image.";
                    _logger.Warn(exception, message);
                    throw new InternalServerException(message);
                }
            }
        }
        [HttpPut("{id:int}")]
        public async Task<ItemImageViewModel> Put(int id, [FromBody]ItemImageViewModel itemImageViewModel)
        {
            itemImageViewModel.Id = id;
            return await base.PutAsync(itemImageViewModel);
        }
        [HttpGet]
        public async Task<ItemImageIndexViewModel> Index([FromQuery(Name = "itemId")]int itemId)
        {
            var spec = new Specification<ItemImage>((i => i.RelatedId == itemId));
            spec.Description = $"ItemImage with RelatedId={itemId}";
            return await base.IndexNotPagedAsync(spec);
        }
        [HttpGet("{id:int}/file")]
        public async Task<ActionResult> GetFile(int id)
        {
            try
            {
                var image = await _serivce.GetSingleAsync(new Specification<ItemImage>(id));
                if (image != null)
                    return new FileStreamResult(_serivce.GetStream(image), image.ContentType);
                return null;
            }
            catch (Exception exception)
            {
                string message = $"Error retrieving item image id = {id}.";
                _logger.Warn(exception, message);
                throw new InternalServerException(message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ItemImageViewModel> Get(int id)
        {
            return await base.GetAsync(new Specification<ItemImage>(id));
        }
        [HttpGet("{id:int}/details")]
        public async Task<ItemImageDetailViewModel> GetDetal(int id)
        {
            return await base.GetDetailAsync(new ItemImageDetailSpecification(id));
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteAsync(new ItemImageDetailSpecification(id));
        }
        protected bool ValidateImageFile(IFormFile imageFile)
        {
            int relatedId = 0;
            Int32.TryParse(imageFile.FileName, out relatedId);
            if (imageFile == null || imageFile.Length == 0)
                throw new EntityValidationError("Image file not provided");
            if (imageFile.Length > 10 * 1024 * 1024)
                throw new EntityValidationError($"The image exceeds 10 MB.");
            if (!allowedContentTypes.Contains(imageFile.ContentType))
                throw new EntityValidationError($"Unsupported content type: { imageFile.ContentType }");
            if (!_itemService.Exist(new Specification<Item>(relatedId)))
                throw new EntityNotFoundException($"Error saving image: item with Id {relatedId} does not exist");
            return true;
        }
    }
}