using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class OrderItemsControllerTests : ControllerTests<OrderItem, OrderItemViewModel, IOrderItemsController, IOrderItemService>
    {
        public OrderItemsControllerTests(ITestOutputHelper output) : base(output)
        {
        }

        protected override async Task AssertCreateSuccessAsync(OrderItemViewModel expected, OrderItemViewModel actual)
        {
            var spec = new EntitySpecification<Order>(actual.OrderId);
            spec.AddInclude("OrderItems.ItemVariant");
            var order = await Context.ReadSingleBySpecAsync(Logger, spec);
            decimal expectedSum = (from i in order.OrderItems select i.ItemVariant.Price * i.Number).Sum();
            Assert.Equal(expectedSum, order.Sum);
            Assert.Contains(order.OrderItems, (i => i.Id == actual.Id));
            await base.AssertCreateSuccessAsync(expected, actual);
        }

        protected override async Task AssertUpdateSuccessAsync(OrderItem beforeUpdate, OrderItemViewModel expected, OrderItemViewModel actual)
        {
            await base.AssertUpdateSuccessAsync(beforeUpdate, expected, actual);
            await AssertCreateSuccessAsync(expected, actual);
        }

        protected override IEnumerable<OrderItem> GetCorrectEntitiesToCreate()
        {
            return new List<OrderItem>()
            {
                new OrderItem() { ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id, Number = 2, OrderId = Data.Orders.JohnInJohnsStore.Id }
            };
        }

        protected override IEnumerable<OrderItemViewModel> GetCorrectViewModelsToUpdate()
        {
            var orderItem = Data.OrderItems.JohnInJohnsStore1;
            orderItem.Number = 99;
            return new List<OrderItemViewModel>()
            {
                ToViewModel(orderItem)
            };
        }

        public override async Task DeleteAsync()
        {
            var actual = Context.Set<OrderItem>().LastOrDefault();
            var spec = new EntitySpecification<Order>(actual.OrderId);
            spec.AddInclude("OrderItems.ItemVariant");
            var order = await Context.ReadSingleBySpecAsync(Logger, spec);
            decimal initialSum = order.Sum;
            decimal expectedSum = initialSum - actual.ItemVariant.Price * actual.Number;
            await base.DeleteAsync();
            Assert.Equal(expectedSum, order.Sum);
        }

        protected override OrderItemViewModel ToViewModel(OrderItem entity)
        {
            return new OrderItemViewModel()
            {
                Id = entity.Id,
                ItemVariantId = entity.ItemVariantId,
                OrderId = entity.OrderId,
                Number = entity.Number
            };
        }
    }
}
