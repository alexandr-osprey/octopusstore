using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
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

        public override async Task RelinkRelatedAsync(int id, int idToRelinkTo)
        {
            var categoryCharacteristicValues = await Context.EnumerateRelatedEnumAsync(Logger, new EntitySpecification<Characteristic>(id), b => b.CharacteristicValues);
            foreach (var value in categoryCharacteristicValues)
                value.CharacteristicId = idToRelinkTo;
            await Context.SaveChangesAsync(Logger, "Relink Characteristic");
        }

        protected override async Task ValidateCreateWithExceptionAsync(Characteristic characteristic)
        {
            await base.ValidateCreateWithExceptionAsync(characteristic);
            await ValidateUpdateWithExceptionAsync(characteristic);
            if (!await Context.ExistsBySpecAsync(Logger, new EntitySpecification<Category>(characteristic.CategoryId)))
                throw new EntityValidationException("Category does not exist");
        }

        protected override async Task ValidateUpdateWithExceptionAsync(Characteristic characteristic)
        {
            await base.ValidateUpdateWithExceptionAsync(characteristic);
            if (string.IsNullOrWhiteSpace(characteristic.Title))
                throw new EntityValidationException("Title not specified");
        }
    }
}
