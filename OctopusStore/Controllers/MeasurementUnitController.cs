using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Mvc;
using OctopusStore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/measurementUnits")]
    public class MeasurementUnitController
        : ReadController<
            IMeasurementUnitService,
            MeasurementUnit, 
            MeasurementUnitViewModel, 
            MeasurementUnitDetailViewModel, 
            MeasurementUnitIndexViewModel>
    {
        public MeasurementUnitController(IMeasurementUnitService measurementUnitService, IAppLogger<IEntityController<MeasurementUnit>> logger)
            : base(measurementUnitService, logger)
        {  }

        // GET: api/<controller>
        [HttpGet]
        public async Task<MeasurementUnitIndexViewModel> Index()
        {
            return await base.IndexNotPagedAsync(new Specification<MeasurementUnit>());
        }
    }
}
