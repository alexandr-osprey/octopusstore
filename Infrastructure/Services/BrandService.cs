using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infrastructure.Services
{
    public class BrandService : Service<Brand>, IBrandService
    {
        public BrandService(
            IAsyncRepository<Brand> repository,
            IAppLogger<Service<Brand>> logger)
            : base(repository, logger)
        {  }
    }
}
