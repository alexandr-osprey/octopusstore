using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
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
            Controller.ScopedParameters.ClaimsPrincipal = Users.JenniferPrincipal;

            var actual = await Controller.IndexAsync(page, pageSize, null, null, null);
            var orders = Data.Orders.Entities.Where(o => o.OwnerId == Users.JenniferId);
            int totalCount = orders.Count();
            orders = orders.Skip(pageSize * (page - 1)).Take(pageSize);
            
            var expected = new IndexViewModel<OrderViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in orders select new OrderViewModel(i));
            Equal(expected, actual);

            // try to index as user someoneelse's orders
            var actual2 = await Controller.IndexAsync(page, pageSize, null, null, Users.JohnId);
            Equal(expected, actual2);
        }

        [Fact]
        public async Task IndexAdminWithoutFiltersAsync()
        {
            int page = 2;
            int pageSize = 3;
            Controller.ScopedParameters.ClaimsPrincipal = Users.AdminPrincipal;

            var actual = await Controller.IndexAsync(page, pageSize, null, null, null);
            int totalCount = Data.Orders.Entities.Count();
            var orders = Data.Orders.Entities.Skip(pageSize * (page - 1)).Take(pageSize);

            var expected = new IndexViewModel<OrderViewModel>(
                page,
                GetPageCount(totalCount, pageSize),
                totalCount,
                from i in orders select new OrderViewModel(i));
            Equal(expected, actual);
        }


        protected override IEnumerable<Order> GetCorrectEntitiesToCreate()
        {
            return new List<Order>()
            {
                new Order() { StoreId = Data.Stores.Jennifers.Id }
            };
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
            var order = Data.Orders.Jen2000;
            order.Sum = 999;
            var order2 = Data.Orders.John1000;
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
                StoreId = entity.StoreId,
                DateTimeCancelled = entity.DateTimeCancelled,
                DateTimeCreated = entity.DateTimeCreated,
                DateTimeFinished = entity.DateTimeFinished,
                Status = entity.Status,
                Sum = entity.Sum
            };
        }
    }
}
