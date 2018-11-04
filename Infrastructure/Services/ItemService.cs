using ApplicationCore.Entities;
using System.Threading.Tasks;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using ApplicationCore.Identity;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;

namespace Infrastructure.Services
{
    public class ItemService: Service<Item>, IItemService
    {
        protected IItemImageService ItemImageService { get; }
        protected ICategoryService CategoryService { get; }

        public ItemService(
            StoreContext context,
            IIdentityService identityService,
            IItemImageService itemImageService,
            ICategoryService categoryService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Item> authoriationParameters,
            IAppLogger<Service<Item>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            this.ItemImageService = itemImageService;
            this.CategoryService = categoryService;
        }

        public async Task<ItemIndexSpecification> GetIndexSpecificationByParameters(int page, int pageSize, string title, int? categoryId, int? storeId, int? brandId)
        {
            return new ItemIndexSpecification(page, pageSize, title, await GetCategoriesAsync(categoryId), storeId, brandId);
        }

        public override async Task DeleteRelatedEntitiesAsync(Item item)
        {
            var imageDeleteSpec = new Specification<ItemImage>(i => i.RelatedId == item.Id)
            {
                Description = $"ItemImage with item id={item.Id}"
            };
            await ItemImageService.DeleteAsync(imageDeleteSpec);
            await base.DeleteRelatedEntitiesAsync(item);
        }

        private async Task<IEnumerable<Category>> GetCategoriesAsync(int? categoryId)
        {
            return categoryId.HasValue
                ? await CategoryService.EnumerateSubcategoriesAsync(new EntitySpecification<Category>(categoryId.Value))
               : new List<Category>();
        }

        protected override async Task ValidateCreateWithExceptionAsync(Item item)
        {
            await ValidateUpdateWithExceptionAsync(item);
            var category = await Context.ReadByKeyAsync<Category, Service<Item>>(Logger, item.CategoryId, false)
                ?? throw new EntityValidationException($"Category with Id {item.CategoryId} does not exist. ");
            if (!category.CanHaveItems)
                throw new EntityValidationException($"Category with Id {item.CategoryId} can't have items. ");
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Store>(item.StoreId)))
                throw new EntityValidationException($"Store with Id {item.StoreId}  does not exist. ");
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Brand>(item.BrandId)))
                throw new EntityValidationException($"Brand with Id {item.BrandId}  does not exist. ");
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<MeasurementUnit>(item.MeasurementUnitId)))
                throw new EntityValidationException($"MeasurementUnit with Id {item.MeasurementUnitId}  does not exist. ");
            await base.ValidateCreateWithExceptionAsync(item);
        }

        protected override async Task ValidateUpdateWithExceptionAsync(Item item)
        {
            await base.ValidateUpdateWithExceptionAsync(item);
            if (string.IsNullOrWhiteSpace(item.Title))
                throw new EntityValidationException("Incorrect title");
            if (string.IsNullOrWhiteSpace(item.Description))
                throw new EntityValidationException("Incorrect description");
        }
    }
}
