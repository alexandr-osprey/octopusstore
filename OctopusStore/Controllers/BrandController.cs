using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.Specifications;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BrandsController 
        : ReadController<
            IBrandService, 
            Brand, 
            BrandViewModel, 
            BrandDetailViewModel,
            BrandIndexViewModel>
    {
        public BrandsController(IBrandService brandService, IAppLogger<IEntityController<Brand>> logger)
            : base(brandService, logger)
        {   }

        // GET: api/<controller>
        [HttpGet]
        public async Task<BrandIndexViewModel> Index()
        {
            return await base.IndexNotPagedAsync(new Specification<Brand>());
        }

        // GET api/<controller>/5
        [HttpGet("{id:int}")]
        public async Task<BrandViewModel> Get(int id)
        {
            return await base.GetAsync(new Specification<Brand>(id));
        }
        [HttpGet("{id:int}/details")]
        public async Task<BrandDetailViewModel> GetDetail(int id)
        {
            return await base.GetDetailAsync(new BrandDetailSpecification(id));
        }
    }
}
