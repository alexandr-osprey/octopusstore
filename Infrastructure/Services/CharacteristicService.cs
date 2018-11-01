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
        protected readonly ICategoryService _сategoryService;

        public CharacteristicService(
            StoreContext context,
            IIdentityService identityService,
            ICategoryService categoryService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Characteristic> authoriationParameters,
            IAppLogger<Service<Characteristic>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            _сategoryService = categoryService;
        }

        public async Task<IEnumerable<Characteristic>> EnumerateAsync(Specification<Item> spec)
        {
            var categories = await _сategoryService.EnumerateParentCategoriesAsync(spec);
            return await EnumerateAsync(new CharacteristicByCategoryIdsSpecification(from c in categories select c.Id));
        }

        public async Task<IEnumerable<Characteristic>> EnumerateByCategoryAsync(Specification<Category> spec)
        {
            var categories = await _сategoryService.EnumerateParentCategoriesAsync(spec);
            return await EnumerateAsync(new CharacteristicByCategoryIdsSpecification(from c in categories select c.Id));
        }

        protected override async Task ValidateCreateWithExceptionAsync(Characteristic characteristic)
        {
            await base.ValidateCreateWithExceptionAsync(characteristic);
            await ValidateUpdateWithExceptionAsync(characteristic);
            if (!await _context.ExistsBySpecAsync(_logger, new EntitySpecification<Category>(characteristic.CategoryId)))
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
