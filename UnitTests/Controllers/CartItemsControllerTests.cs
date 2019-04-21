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
            var cartItems = await _context.Set<CartItem>().Where(i => i.OwnerId == Users.JohnId).ToListAsync();
            var expected = IndexViewModel<CartItemViewModel>.FromEnumerableNotPaged(from c in cartItems select ToViewModel(c));
            var actual = await _controller.IndexAsync();
            Equal(expected, actual);
        }

        [Fact]
        public async Task IndexThumbnailsAsync()
        {
            var cartItems = await _context.Set<CartItem>().Where(i => i.OwnerId == Users.JohnId).ToListAsync();
            var expected = IndexViewModel<CartItemThumbnailViewModel>.FromEnumerableNotPaged(from c in cartItems select new CartItemThumbnailViewModel(c));
            var actual = await _controller.IndexThumbnailsAsync();
            actual.Entities = actual.Entities.OrderBy(i => i.Id);
            expected.Entities = expected.Entities.OrderBy(i => i.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task AddToCartAsync()
        {
            var existing = _data.CartItems.JohnIPhoneXR64FromJohns;
            var expected = ToViewModel(existing);
            expected.Number += 5;
            var actual = await _controller.AddToCartAsync(new CartItemViewModel() { ItemVariantId = existing.ItemVariantId, Number = 5 });
            //Equal(expected., actual);
            Assert.Equal(expected.Number, actual.Number);
        }

        [Fact]
        public async Task RemoveFromCartAsync()
        {
            var existing = _data.CartItems.JohnIPhoneXR64FromJohns;
            var actual = await _controller.RemoveFromCartAsync(new CartItemViewModel() { ItemVariantId = existing.ItemVariantId, Number = existing.Number });
        }

        protected override Task AssertUpdateSuccessAsync(CartItem beforeUpdate, CartItemViewModel expected, CartItemViewModel actual)
        {
            Assert.Equal(expected.Number, actual.Number);
            Assert.Equal(beforeUpdate.ItemVariantId, actual.ItemVariantId);
            Assert.Equal(beforeUpdate.Id, actual.Id);
            return Task.CompletedTask;
        }

        protected override IEnumerable<CartItem> GetCorrectEntitiesToCreate()
        {
            return new List<CartItem>()
            {
                new CartItem()
                {
                    ItemVariantId = _data.ItemVariants.ReebokFastTempoBlack42.Id,
                    Number = 4
                },
                new CartItem()
                {
                    ItemVariantId = _data.ItemVariants.UCBDress2WhiteM.Id,
                    Number = 5
                }
            };
        }

        protected override IEnumerable<CartItemViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<CartItemViewModel>() { new CartItemViewModel() { Id = _data.CartItems.JenniferIPhoneXR64FromJohns.Id, ItemVariantId = 999, Number = 50 } };
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
