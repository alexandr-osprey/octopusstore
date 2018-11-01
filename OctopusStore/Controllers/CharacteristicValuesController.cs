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
        public async Task<IndexViewModel<CharacteristicValueViewModel>> Index(int categoryId)
        {
            return await base.IndexByFunctionNotPagedAsync(_service.EnumerateByCategoryAsync, new EntitySpecification<Category>(categoryId));
        }

        [HttpPut("{id:int}")]
        public async Task<CharacteristicValueViewModel> UpdateAsync(int id, [FromBody]CharacteristicValueViewModel characteristicValueViewModel)
        {
            characteristicValueViewModel.Id = id;
            return await base.UpdateAsync(characteristicValueViewModel);
        }

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<Response> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
