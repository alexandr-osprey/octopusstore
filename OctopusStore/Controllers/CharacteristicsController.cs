﻿using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CharacteristicsController: CRUDController<ICharacteristicService, Characteristic, CharacteristicViewModel>, ICharacteristicsController
    {
        public CharacteristicsController(
            ICharacteristicService service,
            IScopedParameters scopedParameters,
            IAppLogger<IController<Characteristic, CharacteristicViewModel>> logger)
           : base(service, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<CharacteristicViewModel> CreateAsync([FromBody]CharacteristicViewModel characteristicViewModel) => await base.CreateAsync(characteristicViewModel);

        [HttpGet("{id:int}")]
        public override async Task<CharacteristicViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet]
        [HttpGet("/api/categories/{categoryId:int}/characteristics")]
        public async Task<IndexViewModel<CharacteristicViewModel>> IndexAsync([FromQuery(Name = "categoryId")]int categoryId) => await base.IndexByFunctionNotPagedAsync(Service.EnumerateByCategoryAsync, new EntitySpecification<Category>(categoryId));

        [HttpPut]
        public override async Task<CharacteristicViewModel> UpdateAsync([FromBody]CharacteristicViewModel characteristicViewModel) => await base.UpdateAsync(characteristicViewModel);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public override async Task<Response> CheckUpdateAuthorizationAsync(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
