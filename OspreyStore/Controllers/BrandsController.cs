using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BrandsController: CRUDController<IBrandService, Brand, BrandViewModel>, IBrandsController
    {
        public BrandsController(
            IBrandService  brandService,
            IActivatorService activatorService,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<Brand, BrandViewModel>> logger)
           : base(brandService, activatorService, identityService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<BrandViewModel> CreateAsync([FromBody]BrandViewModel brandViewModel) => await base.CreateAsync(brandViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<BrandViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<BrandViewModel>> IndexAsync() => await base.IndexNotPagedAsync(new EntitySpecification<Brand>());

        [HttpPut]
        public override async Task<BrandViewModel> UpdateAsync([FromBody]BrandViewModel brandViewModel) => await base.UpdateAsync(brandViewModel);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<Response> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
