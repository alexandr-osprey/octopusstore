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
                new Order() { ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id,  Number = 1, Sum = 0 },
                new Order() { ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id,  Number = 2, Sum = 0 },
                new Order() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, Number = 2, Sum = 1 },
            };
        }

        protected override async Task AssertCreateSuccessAsync(Order created)
        {
            await base.AssertCreateSuccessAsync(created);
            Assert.Equal(created.ItemVariant.Price * created.Number, created.Sum);
        }

        protected override IEnumerable<Order> GetIncorrectNewEntites()
        {
            return new List<Order>()
            {
                //new Order() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, Number = 2, Sum = -100 },

                new Order() { ItemVariantId = Data.ItemVariants.IPhone664GB.Id, Number = 0, Sum = 100 },
                new Order() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, Number = -2, Sum = 100 },

                new Order() { ItemVariantId = 9999, Number = 1, Sum = 111 },
            };
        }

        protected override Specification<Order> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Order>(b => b.Number > 1);
        }

        protected override IEnumerable<Order> GetCorrectEntitesForUpdate()
        {
            Data.Orders.JenInJohnsStore.Sum = 999;
            Data.Orders.JenInJohnsStore.Number = 100;
            return new List<Order>() { Data.Orders.JenInJohnsStore };
        }

        protected override IEnumerable<Order> GetIncorrectEntitesForUpdate()
        {
            Data.Orders.JenInJohnsStore.Sum = -1;
            Data.Orders.JenInJohnsStoreCancelled.Status = OrderStatus.Created;
            Data.Orders.JenInJohnsStoreFinished.Status = OrderStatus.Cancelled;
            Data.Orders.JohnInJohnsStore.Number = 0;
            Data.Orders.JohnInJensStore.Number = -1;
            Data.Orders.JenInJensStore.ItemVariantId = Data.ItemVariants.IPhone632GB.Id;
            return new List<Order>()
            {
                Data.Orders.JenInJohnsStore,
                Data.Orders.JenInJohnsStoreCancelled,
                Data.Orders.JenInJohnsStoreFinished,
                Data.Orders.JohnInJohnsStore,
                Data.Orders.JohnInJensStore,
                Data.Orders.JenInJensStore
            };
        }

        [Fact]
        public async Task SetOrderFinishedStatusAsync()
        {
            var order = Data.Orders.JohnInJensStore;
            var cancelledStatusTime = DateTime.UtcNow;
            await Service.SetStatusAsync(order.Id, OrderStatus.Finished);
            Assert.True(order.DateTimeFinished >= cancelledStatusTime);
            Assert.Equal(OrderStatus.Finished, order.Status);
        }

        [Fact]
        public async Task SetOrderCancelledStatusAsync()
        {
            var order = Data.Orders.JohnInJensStore;
            var finishedStatusTime = DateTime.UtcNow;
            await Service.SetStatusAsync(order.Id, OrderStatus.Cancelled);
            Assert.True(order.DateTimeCancelled >= finishedStatusTime);
            Assert.Equal(OrderStatus.Cancelled, order.Status);
        }

        [Fact]
        public async Task TryToSetOrderStatusFromCancelledAsync()
        {
            var order = Data.Orders.JenInJohnsStoreCancelled;
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Created));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Finished));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Cancelled));
        }

        [Fact]
        public async Task TryToSetOrderStatusFromFinishedAsync()
        {
            var order = Data.Orders.JenInJohnsStoreFinished;
            await Service.SetStatusAsync(order.Id, OrderStatus.Finished);
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Created));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Finished));
            await Assert.ThrowsAsync<EntityValidationException>(() => Service.SetStatusAsync(order.Id, OrderStatus.Cancelled));
        }
    }
}
