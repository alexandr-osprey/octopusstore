using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Threading.Tasks;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;
using ApplicationCore.Interfaces.Controllers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using Infrastructure.Data.SampleData;

namespace UnitTests.Controllers
{
    public class CartItemsControllerTests : ControllerTests<CartItem, CartItemViewModel, ICartItemsController, ICartItemService>
    {
        public CartItemsControllerTests(ITestOutputHelper output): base(output)
        {
        }

        [Fact]
        public async Task IndexAsync()
        {
            var cartItems = await Context.Set<CartItem>().Where(i => i.OwnerId == Users.JohnId).ToListAsync();
            var expected = IndexViewModel<CartItemViewModel>.FromEnumerableNotPaged(from c in cartItems select ToViewModel(c));
            var actual = await Controller.Index();
            Equal(expected, actual);
        }

        [Fact]
        public async Task AddToCartAsync()
        {
            var existing = Data.CartItems.JohnIphone32;
            var expected = ToViewModel(existing);
            expected.Number += 5;
            var actual = await Controller.AddToCartAsync(new CartItemViewModel() { ItemVariantId = existing.ItemVariantId, Number = 5 });
            Equal(expected, actual);
        }

        [Fact]
        public async Task RemoveFromCartAsync()
        {
            var existing = Data.CartItems.JohnIphone32;
            var actual = await Controller.RemoveFromCartAsync(new CartItemViewModel() { ItemVariantId = existing.ItemVariantId, Number = existing.Number });
        }

        protected override void AssertUpdateSuccess(CartItem beforeUpdate, CartItemViewModel expected, CartItemViewModel actual)
        {
            Assert.Equal(expected.Number, actual.Number);
            Assert.Equal(beforeUpdate.ItemVariantId, actual.ItemVariantId);
            Assert.Equal(beforeUpdate.Id, actual.Id);
        }

        protected override IEnumerable<CartItem> GetCorrectEntitiesToCreate()
        {
            return new List<CartItem>()
            {
                new CartItem()
                {
                    ItemVariantId = Data.ItemVariants.JacketBlack.Id,
                    Number = 4
                },
                new CartItem()
                {
                    ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id,
                    Number = 5
                }
            };
        }

        protected override IEnumerable<CartItem> GetCorrectEntitiesToUpdate()
        {
            var cartItems = Data.CartItems.Entities;
            cartItems.ForEach(c => c.Number = 99);
            cartItems.ForEach(c => c.ItemVariantId = 999);
            return cartItems;
        }

        protected override CartItemViewModel ToViewModel(CartItem entity)
        {
            return new CartItemViewModel()
            {
                Id = entity.Id,
                ItemVariantId = entity.ItemVariantId,
                Number = entity.Number
            };
        }
    }
}
