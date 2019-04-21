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
                new Order() { ItemVariantId = _data.ItemVariants.IPhone8Plus128GBBlack.Id,  Number = 1, Sum = 0 },
                new Order() { ItemVariantId = _data.ItemVariants.IPhone8Plus128GBWhite.Id,  Number = 2, Sum = 0 },
                new Order() { ItemVariantId = _data.ItemVariants.IPhone8Plus64GBBlack.Id, Number = 2, Sum = 1 },
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

                new Order() { ItemVariantId = _data.ItemVariants.IPhone8Plus128GBBlack.Id, Number = 0, Sum = 100 },
                new Order() { ItemVariantId = _data.ItemVariants.IPhone8Plus128GBWhite.Id, Number = -2, Sum = 100 },

                new Order() { ItemVariantId = 9999, Number = 1, Sum = 111 },
            };
        }

        protected override Specification<Order> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Order>(b => b.Number > 1);
        }

        protected override IEnumerable<Order> GetCorrectEntitesForUpdate()
        {
            _data.Orders.JenInJohnsStore.Sum = 999;
            _data.Orders.JenInJohnsStore.Number = 100;
            return new List<Order>() { _data.Orders.JenInJohnsStore };
        }

        protected override IEnumerable<Order> GetIncorrectEntitesForUpdate()
        {
            _data.Orders.JenInJohnsStore.Sum = -1;
            _data.Orders.JenInJohnsStoreCancelled.Status = OrderStatus.Created;
            _data.Orders.JenInJohnsStoreFinished.Status = OrderStatus.Cancelled;
            _data.Orders.JohnInJohnsStore.Number = 0;
            _data.Orders.JohnInJensStore.Number = -1;
            _data.Orders.JenInJensStore.ItemVariantId = _data.ItemVariants.IPhone8Plus128GBWhite.Id;
            return new List<Order>()
            {
                _data.Orders.JenInJohnsStore,
                _data.Orders.JenInJohnsStoreCancelled,
                _data.Orders.JenInJohnsStoreFinished,
                _data.Orders.JohnInJohnsStore,
                _data.Orders.JohnInJensStore,
                _data.Orders.JenInJensStore
            };
        }

        [Fact]
        public async Task SetOrderFinishedStatusAsync()
        {
            var order = _data.Orders.JohnInJensStore;
            var cancelledStatusTime = DateTime.UtcNow;
            await _service.SetStatusAsync(order.Id, OrderStatus.Finished);
            Assert.True(order.DateTimeFinished >= cancelledStatusTime);
            Assert.Equal(OrderStatus.Finished, order.Status);
        }

        [Fact]
        public async Task SetOrderCancelledStatusAsync()
        {
            var order = _data.Orders.JohnInJensStore;
            var finishedStatusTime = DateTime.UtcNow;
            await _service.SetStatusAsync(order.Id, OrderStatus.Cancelled);
            Assert.True(order.DateTimeCancelled >= finishedStatusTime);
            Assert.Equal(OrderStatus.Cancelled, order.Status);
        }

        [Fact]
        public async Task TryToSetOrderStatusFromCancelledAsync()
        {
            var order = _data.Orders.JenInJohnsStoreCancelled;
            await Assert.ThrowsAsync<EntityValidationException>(() => _service.SetStatusAsync(order.Id, OrderStatus.Created));
            await Assert.ThrowsAsync<EntityValidationException>(() => _service.SetStatusAsync(order.Id, OrderStatus.Finished));
            await Assert.ThrowsAsync<EntityValidationException>(() => _service.SetStatusAsync(order.Id, OrderStatus.Cancelled));
        }

        [Fact]
        public async Task TryToSetOrderStatusFromFinishedAsync()
        {
            var order = _data.Orders.JenInJohnsStoreFinished;
            await _service.SetStatusAsync(order.Id, OrderStatus.Finished);
            await Assert.ThrowsAsync<EntityValidationException>(() => _service.SetStatusAsync(order.Id, OrderStatus.Created));
            await Assert.ThrowsAsync<EntityValidationException>(() => _service.SetStatusAsync(order.Id, OrderStatus.Finished));
            await Assert.ThrowsAsync<EntityValidationException>(() => _service.SetStatusAsync(order.Id, OrderStatus.Cancelled));
        }
    }
}
