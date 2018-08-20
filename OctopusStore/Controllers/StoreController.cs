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
    [Route("api/Stores")]
    public class StoreController 
        : ReadWriteController<
            IStoreService, 
            Store, 
            StoreViewModel, 
            StoreDetailViewModel, 
            StoreIndexViewModel>
    {
        public StoreController(IStoreService storeService, IAppLogger<IEntityController<Store>> logger)
            : base(storeService, logger)
        {  }

        // GET: api/<controller>
        [HttpGet]
        public async Task<StoreIndexViewModel> Index(
            [FromQuery(Name = "page")]int? page,
            [FromQuery(Name = "pageSize")]int? pageSize,
            [FromQuery(Name = "title")]string title)
        {
            pageSize = pageSize ?? _defaultTake;
            page = page ?? 1;
            return await base.IndexAsync(new StoreIndexSpecification(page.Value, pageSize.Value, title));
        }

        // GET api/<controller>/5
        [HttpGet("{id:int}")]
        public async Task<StoreViewModel> Get(int id)
        {
            return await base.GetAsync(new Specification<Store>(id));
        }
        [HttpGet("{id:int}/details")]
        public async Task<StoreDetailViewModel> GetDetail(int id)
        {
            return await base.GetDetailAsync(new StoreDetailSpecification(id));
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<StoreViewModel> Post([FromBody]StoreViewModel storeViewModel)
        {
            storeViewModel.RegistrationDate = System.DateTime.Now;
            return await base.PostAsync(storeViewModel);
        }
        // PUT api/<controller>/5
        [HttpPut("{id:int}")]
        public async Task<StoreViewModel> Put(int id, [FromBody]StoreViewModel storeViewModel)
        {
            storeViewModel.Id = id;
            return await base.PutAsync(storeViewModel);
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await base.DeleteAsync(new StoreDetailSpecification(id));
        }
    }
}
