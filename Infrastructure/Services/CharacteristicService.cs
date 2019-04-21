using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CharacteristicService: Service<Characteristic>, ICharacteristicService
    {
        protected ICategoryService CategoryService { get; }

        public CharacteristicService(
            StoreContext context,
            IIdentityService identityService,
            ICategoryService categoryService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Characteristic> authoriationParameters,
            IAppLogger<Service<Characteristic>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            CategoryService = categoryService;
        }

        public async Task<IEnumerable<Characteristic>> EnumerateAsync(Specification<Item> spec)
        {
            var categories = await CategoryService.EnumerateParentCategoriesAsync(spec);
            return await EnumerateAsync(new CharacteristicByCategoryIdsSpecification(from c in categories select c.Id));
        }

        public async Task<IEnumerable<Characteristic>> EnumerateByCategoryAsync(Specification<Category> spec)
        {
            var categories = await CategoryService.EnumerateParentCategoriesAsync(spec);
            return await EnumerateAsync(new CharacteristicByCategoryIdsSpecification(from c in categories select c.Id));
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<Characteristic> entityEntry)
        {
            await base.ValidateWithExceptionAsync(entityEntry);
            var characteristic = entityEntry.Entity;
            if (string.IsNullOrWhiteSpace(characteristic.Title))
                throw new EntityValidationException("Title not specified");
            if (IsPropertyModified(entityEntry, c => c.CategoryId, false) 
                && !await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Category>(characteristic.CategoryId)))
                throw new EntityValidationException("Category does not exist");
        }
    }
}
