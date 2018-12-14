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
    public class OrdersController: CRUDController<IOrderService, Order, OrderViewModel>, IOrdersController
    {
        protected ICartItemService CartItemService { get; }
        protected IOrderItemService OrderItemService { get; }

        public OrdersController(
            IOrderService orderService,
            ICartItemService cartItemService,
            IOrderItemService orderItemService,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<Order, OrderViewModel>> logger)
           : base(orderService, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<OrderViewModel> CreateAsync([FromBody]OrderViewModel orderViewModel)
        {
            if (orderViewModel == null)
                throw new EntityValidationException("No order view model provided");
            var spec = new CartItemStoreSpecification(ScopedParameters.ClaimsPrincipal.Identity.Name, orderViewModel.StoreId);
            var cartItems = await CartItemService.EnumerateAsync(spec);
            if (!cartItems.Any())
                throw new EntityValidationException($"No cart items with store {orderViewModel.StoreId}");
            orderViewModel.Sum = (from i in cartItems select i.ItemVariant.Price).Sum();
            var createdOrder = await base.CreateAsync(orderViewModel);
            foreach(var cartItem in cartItems)
                await OrderItemService.CreateAsync(new OrderItem(cartItem, createdOrder.Id));
            return createdOrder;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<OrderViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet]
        public async Task<IndexViewModel<OrderViewModel>> IndexAsync() => await base.IndexNotPagedAsync(new EntitySpecification<Order>());

        [HttpPut]
        public override async Task<OrderViewModel> UpdateAsync([FromBody]OrderViewModel brandViewModel) => await base.UpdateAsync(brandViewModel);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<Response> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);
    }
}
