using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : CRUDController<IMeasurementUnitService, MeasurementUnit, MeasurementUnitViewModel>, IMeasurementUnitsController
    {
        public MeasurementUnitsController(
            IMeasurementUnitService measurementUnitService,
            IActivatorService activatorService,
            IAuthorizationService authorizationService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<MeasurementUnit, MeasurementUnitViewModel>> logger)
           : base(measurementUnitService, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<MeasurementUnitViewModel> CreateAsync([FromBody]MeasurementUnitViewModel measurementUnitViewModel) => await base.CreateAsync(measurementUnitViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<MeasurementUnitViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<MeasurementUnitViewModel>> IndexAsync() => await base.IndexNotPagedAsync(new Specification<MeasurementUnit>());

        [HttpPut]
        public override async Task<MeasurementUnitViewModel> UpdateAsync([FromBody]MeasurementUnitViewModel measurementUnitViewModel) => await base.UpdateAsync(measurementUnitViewModel);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
