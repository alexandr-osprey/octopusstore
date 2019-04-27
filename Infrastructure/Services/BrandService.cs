using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services
{
    public class BrandService: Service<Brand>, IBrandService
    {
        public BrandService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<Brand> authoriationParameters,
            IAppLogger<Service<Brand>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<Brand> entry)
        {
            await base.ValidateWithExceptionAsync(entry);
            var brand = entry.Entity;
            var entityEntry = _сontext.Entry(brand);
            if (string.IsNullOrWhiteSpace(brand.Title))
                throw new EntityValidationException("Title not specified");
        }
    }
}
