using ApplicationCore.Entities;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CharacteristicService: Service<Characteristic>, ICharacteristicService
    {
        protected ICategoryService _сategoryService;

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
    }
}
