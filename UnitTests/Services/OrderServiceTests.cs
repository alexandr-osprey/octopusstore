using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data.SampleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class OrderServiceTests : ServiceTests<Order, IOrderService>
    {
        public OrderServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<Order> GetCorrectNewEntites()
        {
            return new List<Order>()
            {
                new Order() { StoreId = Data.Stores.Johns.Id }
            };
        }

        protected override IEnumerable<Order> GetIncorrectNewEntites()
        {
            return new List<Order>()
            {
                new Order() { StoreId = 0 },
                new Order() { StoreId = Data.Stores.Jennifers.Id, Sum = -324 }
            };
        }

        protected override Specification<Order> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Order>(b => b.StoreId == Data.Stores.Johns.Id);
        }

        protected override IEnumerable<Order> GetCorrectEntitesForUpdate()
        {
            Data.Orders.JenInJohnsStore.Sum = 999;
            return new List<Order>() { Data.Orders.JenInJohnsStore };
        }

        protected override IEnumerable<Order> GetIncorrectEntitesForUpdate()
        {
            Data.Orders.JenInJohnsStore.Sum = -1;
            return new List<Order>()
            {
                Data.Orders.JenInJohnsStore
            };
        }

        [Fact]
        public async Task DeleteSingleWithRelatedRelinkAsync()
        {
            var order = Data.Orders.John1000;
            int idToRelinkTo = Data.Orders.Jen2000.Id;
            var orderItems = Data.OrderItems.Entities.Where(i => i.Order == order).ToList();
            await Service.DeleteSingleWithRelatedRelink(order.Id, idToRelinkTo);
            orderItems.ForEach(i => Assert.Equal(i.OrderId, idToRelinkTo));
            Assert.False(Context.Set<Order>().Any(b => b == order));
        }

        [Fact]
        public async Task SetOrderFinishedStatusAsync()
        {
            var order = Data.Orders.John1000;
            var cancelledStatusTime = DateTime.UtcNow;
            await Service.SetStatusAsync(order.Id, OrderStatus.Cancelled);
            Assert.True(order.DateTimeCancelled >= cancelledStatusTime);
        }

        [Fact]
        public async Task SetOrderCancelledStatusAsync()
        {
            var order = Data.Orders.John1000;
            var finishedStatusTime = DateTime.UtcNow;
            await Service.SetStatusAsync(order.Id, OrderStatus.Finished);
            Assert.True(order.DateTimeFinished >= finishedStatusTime);
        }

        [Fact]
        public async Task TryToSetOrderCreatedStatusAsync()
        {
            var order = Data.Orders.John1000;
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Created));
        }

        [Fact]
        public async Task TryToSetOrderStatusFromCancelledAsync()
        {
            var order = Data.Orders.John1000;
            await Service.SetStatusAsync(order.Id, OrderStatus.Cancelled);
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Created));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Finished));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Cancelled));
        }

        [Fact]
        public async Task TryToSetOrderStatusFromFinishedAsync()
        {
            var order = Data.Orders.John1000;
            await Service.SetStatusAsync(order.Id, OrderStatus.Finished);
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Created));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Finished));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Cancelled));
        }
    }
}
