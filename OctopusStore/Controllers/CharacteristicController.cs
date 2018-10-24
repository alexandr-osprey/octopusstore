using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CharacteristicsController
        : CRUDController<
            ICharacteristicService, 
            Characteristic,
            CharacteristicViewModel,
            CharacteristicViewModel>
    {
        public CharacteristicsController(
            ICharacteristicService service, 
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<Characteristic>> logger)
            : base(service, scopedParameters, logger)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<CharacteristicViewModel>> Index([FromQuery(Name = "categoryId")]int categoryId)
        {
            return await CategoryCharacteristicsIndex(categoryId);
        }
        [AllowAnonymous]
        [HttpGet("/api/categories/{categoryId:int}/characteristics")]
        public async Task<IndexViewModel<CharacteristicViewModel>> CategoryCharacteristicsIndex(int categoryId)
        {
            return await base.IndexByFunctionNotPagedAsync(_service.EnumerateByCategoryAsync, new EntitySpecification<Category>(categoryId));
        }
        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<ActionResult> CheckUpdateAuthorization(int id)
        {
            return await base.CheckUpdateAuthorizationAsync(id);
        }
    }
}
