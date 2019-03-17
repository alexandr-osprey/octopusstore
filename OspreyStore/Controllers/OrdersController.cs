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

namespace OspreyStore.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrdersController: CRUDController<IOrderService, Order, OrderViewModel>, IOrdersController
    {

        public OrdersController(
            IOrderService orderService,
            ICartItemService cartItemService,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<Order, OrderViewModel>> logger)
           : base(orderService, activatorService, scopedParameters, logger)
        {
        }

        [HttpPost]
        public override async Task<OrderViewModel> CreateAsync([FromBody]OrderViewModel orderViewModel) => await base.CreateAsync(orderViewModel);

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<OrderViewModel> ReadAsync(int id) => await base.ReadAsync(id);

        [AllowAnonymous]
        [HttpGet("{id:int}/detail")]
        public async Task<OrderDetailViewModel> ReadDetailAsync(int id) => await base.ReadDetailAsync<OrderDetailViewModel>(new OrderDetailSpecification(id));

        [HttpGet("thumbnails/")]
        public async Task<IndexViewModel<OrderThumbnailViewModel>> IndexThumbnailsAsync(int? page, int? pageSize, int? storeId, OrderStatus? orderStatus)
        {
            var spec = new OrderThumbnailIndexSpecification(await Service.GetSpecificationAccordingToAuthorizationAsync(page ?? 1, pageSize ?? DefaultTake, storeId, orderStatus));
            return await base.IndexAsync<OrderThumbnailViewModel>(spec);
        }

        [HttpGet()]
        public async Task<IndexViewModel<OrderViewModel>> IndexAsync(int? page, int? pageSize, int? storeId, OrderStatus? orderStatus)
        {
            var spec = await Service.GetSpecificationAccordingToAuthorizationAsync(page ?? 1, pageSize ?? DefaultTake, storeId, orderStatus);
            return await base.IndexAsync<OrderViewModel>(spec);
        }

        [HttpPut]
        public override async Task<OrderViewModel> UpdateAsync([FromBody]OrderViewModel orderViewModel) => await base.UpdateAsync(orderViewModel);

        [HttpGet("{id:int}/checkUpdateAuthorization")]
        public async Task<Response> CheckUpdateAuthorization(int id) => await base.CheckUpdateAuthorizationAsync(id);

        protected override async Task PopulateViewModelWithRelatedDataAsync<TCustomViewModel>(TCustomViewModel viewModel)
        {
            var order = await Service.ReadSingleAsync(new EntitySpecification<Order>(viewModel.GetHashCode()));
            string email = await Service.IdentityService.GetUserEmail(order.OwnerId);
            viewModel.CustomerEmail = email;
        }
    }
}
