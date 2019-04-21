using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.ViewModels;
using Infrastructure.Data.SampleData;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class OrdersControllerTests: ControllerTests<Order, OrderViewModel, IOrdersController, IOrderService>
    {
        public OrdersControllerTests(ITestOutputHelper output): base(output)
        {
        }

        [Fact]
        public async Task IndexOwnWithoutFiltersAsync()
        {
            int page = 2;
            int pageSize = 3;
            _controller.ScopedParameters.ClaimsPrincipal = Users.JenniferPrincipal;

            var actual = await _controller.IndexAsync(page, pageSize, null, null);
            var orders = _data.Orders.Entities.Where(o => o.OwnerId == Users.JenniferId);
            int totalCount = orders.Count();
            orders = orders.Skip(pageSize * (page - 1)).Take(pageSize);
            
            var expected = new IndexViewModel<OrderViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                orders.Select(o => new OrderViewModel(o) { CustomerEmail = o.OwnerId }));
            Equal(expected, actual);

            // try to index as user someoneelse's orders
            var actual2 = await _controller.IndexAsync(page, pageSize, null, null);
            Equal(expected, actual2);
        }

        [Fact]
        public async Task IndexStoreAsContentAdminWithoutFiltersAsync()
        {
            int page = 1;
            int pageSize = 3;
            int storeId = _data.Stores.Johns.Id;
            _controller.ScopedParameters.ClaimsPrincipal = Users.AdminPrincipal;

            var actual = await _controller.IndexAsync(page, pageSize, storeId, null);
            var ordersTotal = _data.Orders.Entities.Where(o => o.ItemVariant.Item.StoreId == storeId);
            var orders = ordersTotal.Skip(pageSize * (page - 1)).Take(pageSize);
            int totalCount = ordersTotal.Count();

            var expected = new IndexViewModel<OrderViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                orders.Select(o => new OrderViewModel(o) { CustomerEmail = o.OwnerId }));
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexStoreAsStoreAdminWithoutFiltersAsync()
        {
            int page = 1;
            int pageSize = 3;
            int storeId = _data.Stores.Johns.Id;
            _controller.ScopedParameters.ClaimsPrincipal = Users.JohnPrincipal;

            var actual = await _controller.IndexAsync(page, pageSize, storeId, null);
            var ordersTotal = _data.Orders.Entities.Where(o => o.ItemVariant.Item.StoreId == storeId);
            var orders = ordersTotal.Skip(pageSize * (page - 1)).Take(pageSize);
            int totalCount = ordersTotal.Count();

            var expected = new IndexViewModel<OrderViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                orders.Select(o => new OrderViewModel(o) { CustomerEmail = o.OwnerId }));

            Equal(expected, actual);
        }

        [Fact]
        public virtual async Task IndexNotOwnOrdersAsAdminThrowsAuthorizationExceptionAsync()
        {
            int storeId = _data.Stores.Johns.Id;
            _controller.ScopedParameters.ClaimsPrincipal = Users.JenniferPrincipal;
            await Assert.ThrowsAsync<AuthorizationException>(() => _service.GetSpecificationAccordingToAuthorizationAsync(1, 1, storeId, null));
        }

        protected override IEnumerable<Order> GetCorrectEntitiesToCreate()
        {
            return new List<Order>()
            {
                new Order() { ItemVariantId = _data.ItemVariants.ReebokFastTempoWhite42.Id, Number = 1, OwnerId = Users.JohnId }
            };
        }

        protected override Task AssertCreateSuccessAsync(OrderViewModel expected, OrderViewModel actual)
        {
            expected.Sum = _data.ItemVariants.Entities.First(v => v.Id == expected.ItemVariantId).Price * expected.Number;
            expected.DateTimeCreated = actual.DateTimeCreated;
            return base.AssertCreateSuccessAsync(expected, actual);
        }
        //protected override async Task AssertCreateSuccessAsync(OrderViewModel expected, OrderViewModel actual)
        //{
        //    var createdOrder = await Context.Set<Order>()
        //        .Include(o => o.OrderItems)
        //        .FirstOrDefaultAsync(o => o.Id == actual.Id);
        //    var cartItemsForOrder = Data.CartItems.Entities.Where(c => c.OwnerId == Users.JohnId && c.ItemVariant.Item.StoreId == actual.StoreId);
        //    decimal sum = (from c in cartItemsForOrder select c.ItemVariant.Price * c.Number).Sum();
        //    Assert.Equal(sum, actual.Sum);
        //    var createdOrderItems = await Context.Set<OrderItem>().Where(i => i.OrderId == createdOrder.Id).ToListAsync();
        //    foreach(var cartItem in cartItemsForOrder)
        //    {
        //        var orderItem = createdOrderItems.First(i => i.ItemVariantId == cartItem.ItemVariantId);
        //        Assert.Equal(cartItem.Number, orderItem.Number);
        //    }
        //}

        protected override IEnumerable<OrderViewModel> GetCorrectViewModelsToUpdate()
        {
            var order = _data.Orders.JohnInJensStore;
            order.Sum = 999;
            var order2 = _data.Orders.JenInJensStore;
            order2.Status = OrderStatus.Cancelled;
            return new List<OrderViewModel>()
            {
                ToViewModel(order),
                ToViewModel(order2)
            };
        }

        protected override OrderViewModel ToViewModel(Order entity)
        {
            return new OrderViewModel()
            {
                Id = entity.Id,
                ItemVariantId = entity.ItemVariantId,
                Number = entity.Number,
                DateTimeCancelled = entity.DateTimeCancelled,
                DateTimeCreated = entity.DateTimeCreated,
                DateTimeFinished = entity.DateTimeFinished,
                Status = entity.Status,
                Sum = entity.Sum,
                CustomerEmail = entity.OwnerId
            };
        }
    }
}
