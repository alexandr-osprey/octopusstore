using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BrandsController: CRUDController<IBrandService, Brand, BrandViewModel>, IBrandsController
    {
        public BrandsController(
            IBrandService  brandService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<Brand, BrandViewModel>> logger)
           : base(brandService, scopedParameters, logger)
        {
        }

        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<BrandViewModel>> IndexAsync() => await base.IndexNotPagedAsync(new EntitySpecification<Brand>());

        // GET api/<controller>/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<BrandViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<Response> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
