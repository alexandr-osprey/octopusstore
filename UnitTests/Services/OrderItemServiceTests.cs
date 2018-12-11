using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class OrderItemServiceTests : ServiceTests<OrderItem, IOrderItemService>
    {
        public OrderItemServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<OrderItem> GetCorrectNewEntites()
        {
            return new List<OrderItem>()
            {
                new OrderItem() { ItemVariantId = Data.ItemVariants.IPhone664GB.Id, OrderId = Data.Orders.John1000.Id, Number = 1 },
                new OrderItem() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, OrderId = Data.Orders.Jen2000.Id, Number = 2 },
            };
        }

        protected override IEnumerable<OrderItem> GetIncorrectNewEntites()
        {
            return new List<OrderItem>()
            {
                new OrderItem() { ItemVariantId = Data.ItemVariants.IPhone664GB.Id, OrderId = Data.Orders.Jen2000.Id, Number = 1 },
                new OrderItem() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, OrderId = Data.Orders.John1000.Id, Number = 2 },

                new OrderItem() { ItemVariantId = Data.ItemVariants.IPhone664GB.Id, OrderId = Data.Orders.Jen2000.Id, Number = 0 },
                new OrderItem() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, OrderId = Data.Orders.John1000.Id, Number = -2 },

                new OrderItem() { ItemVariantId = 9999, OrderId = Data.Orders.Jen2000.Id, Number = 1 },
                new OrderItem() { ItemVariantId = Data.ItemVariants.JacketBlack.Id, OrderId = 9999, Number = 2 },
            };
        }

        protected override Specification<OrderItem> GetEntitiesToDeleteSpecification()
        {
            return new Specification<OrderItem>(b => b.OrderId == Data.Orders.Jen2000.Id);
        }

        protected override IEnumerable<OrderItem> GetCorrectEntitesForUpdate()
        {
            Data.OrderItems.JenInJohnsStore1.Number = 999;
            return new List<OrderItem>() { Data.OrderItems.JenInJohnsStore1 };
        }

        protected override IEnumerable<OrderItem> GetIncorrectEntitesForUpdate()
        {
            Data.OrderItems.JenInJohnsStore1.Number = 0;
            Data.OrderItems.JenInJohnsStore2.Number = -1;
            Data.OrderItems.John10001.OrderId = Data.Orders.Jen2000.Id;
            Data.OrderItems.John10002.ItemVariantId = Data.Stores.Jennifers.Id;
            return new List<OrderItem>()
            {
                Data.OrderItems.JenInJohnsStore1,
                Data.OrderItems.JenInJohnsStore2,
                Data.OrderItems.John10001,
                Data.OrderItems.John10002
            };
        }
    }
}
