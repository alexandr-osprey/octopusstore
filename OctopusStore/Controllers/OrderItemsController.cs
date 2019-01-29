using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.ViewModels;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Exceptions;
using System.Linq;

namespace OctopusStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrderItemsController: CRUDController<IOrderItemService, OrderItem, OrderItemViewModel>, IOrderItemsController
    {
        protected ICartItemService CartItemService { get; }
        protected IOrderItemService OrderItemService { get; }
        protected IOrderService OrderService { get; }

        public OrderItemsController(
            IOrderItemService orderItemService,
            IOrderService orderService,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<OrderItem, OrderItemViewModel>> logger)
           : base(orderItemService, activatorService, scopedParameters, logger)
        {
            OrderItemService = orderItemService;
            OrderService = orderService;
        }

        [HttpPost]
        public override async Task<OrderItemViewModel> CreateAsync([FromBody]OrderItemViewModel orderItemViewModel)
        {
            var orderItem = await base.CreateAsync(orderItemViewModel);
            await OrderService.RecalculateSumAsync(orderItemViewModel.OrderId);
            return orderItem;
        }

        [HttpGet("{id:int}")]
        public override async Task<OrderItemViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        //[HttpGet]
        //public async Task<IndexViewModel<OrderItemViewModel>> IndexAsync(int? page, int? pageSize, string ownerId)
        //{
        //    if (!await Service.IdentityService.IsContentAdministratorAsync(ScopedParameters.ClaimsPrincipal.Identity.Name))
        //        ownerId = ScopedParameters.ClaimsPrincipal.Identity.Name;
        //    return await base.IndexAsync(new OrderIndexSpecification(page ?? 1, pageSize ?? DefaultTake, storeId, orderStatus, ownerId));
        //}

        [HttpPut]
        public override async Task<OrderItemViewModel> UpdateAsync([FromBody]OrderItemViewModel orderItemViewModel)
        {
            var orderItem = await base.UpdateAsync(orderItemViewModel);
            await OrderService.RecalculateSumAsync(orderItemViewModel.OrderId);
            return orderItem;
        }

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<Response> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);

        [HttpDelete("{id:int}")]
        public override async Task<Response> DeleteAsync(int id)
        {
            var orderItemViewModel = await ReadAsync(id);
            var response = await base.DeleteAsync(id);
            await OrderService.RecalculateSumAsync(orderItemViewModel.OrderId);
            return response;
        }
    }
}
