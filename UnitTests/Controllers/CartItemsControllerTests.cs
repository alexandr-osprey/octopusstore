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
            var cartItems = await _context.Set<CartItem>().AsNoTracking().Where(i => i.OwnerId == johnId).ToListAsync();
            var expected = IndexViewModel<CartItemViewModel>.FromEnumerableNotPaged(from c in cartItems select ToViewModel(c));
            var actual = await _controller.Index();
            Equal(expected, actual);
        }

        [Fact]
        public async Task AddToCartAsync()
        {
            var existing = await _context.Set<CartItem>().FirstAsync(c => c.OwnerId == johnId);
            var expected = ToViewModel(existing);
            expected.Number += 5;
            var actual = await _controller.AddToCartAsync(new CartItemViewModel() { ItemVariantId = existing.ItemVariantId, Number = 5 });
            Equal(expected, actual);

            expected = new CartItemViewModel() { ItemVariantId = 5, Number = 5 };
            actual = await _controller.AddToCartAsync(new CartItemViewModel() { ItemVariantId = 5, Number = 5 });
            await AssertCreateSuccess(expected, actual);
        }

        [Fact]
        public async Task RemoveFromCartAsync()
        {
            var existing = await _context.Set<CartItem>().FirstAsync(c => c.OwnerId == johnId);
            var actual = await _controller.RemoveFromCartAsync(new CartItemViewModel() { ItemVariantId = existing.ItemVariantId, Number = existing.Number });
        }

        protected override Task AssertUpdateSuccessAsync(CartItem beforeUpdate, CartItemViewModel expected, CartItemViewModel actual)
        {
            Assert.Equal(expected.Number, actual.Number);
            Assert.Equal(beforeUpdate.ItemVariantId, actual.ItemVariantId);
            Assert.Equal(beforeUpdate.Id, actual.Id);
            return Task.CompletedTask;
        }

        protected override async Task<IEnumerable<CartItem>> GetCorrectEntitiesToCreateAsync()
        {
            return await Task.FromResult(new List<CartItem>()
            {
                new CartItem()
                {
                    ItemVariantId = 4,
                    Number = 4
                },
                new CartItem()
                {
                    ItemVariantId = 3,
                    Number = 5
                }
            });
        }

        protected override async Task<IEnumerable<CartItem>> GetCorrectEntitiesToUpdateAsync()
        {
            var cartItems = await _context.Set<CartItem>().AsNoTracking().ToListAsync();
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
