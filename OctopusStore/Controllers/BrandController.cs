using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BrandsController 
        : CRUDController<
            IBrandService, 
            Brand, 
            BrandViewModel, 
            BrandDetailViewModel,
            BrandIndexViewModel>
    {
        public BrandsController(
            IBrandService  brandService,
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<Brand>> logger)
            : base(brandService, scopedParameters, logger)
        {
        }

        // GET: api/<controller>
        [AllowAnonymous]
        [HttpGet]
        public async Task<BrandIndexViewModel> Index() => await base.IndexNotPagedAsync(new EntitySpecification<Brand>());

        // GET api/<controller>/5
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<BrandViewModel> Get(int id) => await base.GetAsync(new Specification<Brand>(b => b.Id == id));

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id)
        {
            return await base.CheckUpdateAuthorizationAsync(id);
        }
    }
}
