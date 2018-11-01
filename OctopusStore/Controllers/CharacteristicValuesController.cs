using System.Threading.Tasks;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CharacteristicValuesController: CRUDController<ICharacteristicValueService, CharacteristicValue, CharacteristicValueViewModel>, ICharacteristicValuesController
    {
        public CharacteristicValuesController(
            ICharacteristicValueService service,
            IScopedParameters scopedParameters,
            IAppLogger<IController<CharacteristicValue, CharacteristicValueViewModel>> logger)
           : base(service, scopedParameters, logger)
        {
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<IndexViewModel<CharacteristicValueViewModel>> Index([FromQuery(Name = "categoryId")]int categoryId)
        //{
        //    return await CategoryCharacteristicValuesIndex(categoryId);
        //}

        [AllowAnonymous]
        [HttpGet]
        [HttpGet("/api/categories/{categoryId:int}/characteristicValues")]
        public async Task<IndexViewModel<CharacteristicValueViewModel>> IndexAsync(int categoryId)
        {
            return await base.IndexByFunctionNotPagedAsync(_service.EnumerateByCategoryAsync, new EntitySpecification<Category>(categoryId));
        }

        [HttpPut("{id:int}")]
        public override async Task<CharacteristicValueViewModel> UpdateAsync([FromBody]CharacteristicValueViewModel characteristicValueViewModel)
        {
            return await base.UpdateAsync(characteristicValueViewModel);
        }

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
