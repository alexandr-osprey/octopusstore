using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data.SampleData;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CartItemServiceTests : ServiceTests<CartItem, ICartItemService>
    {
        public CartItemServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        [Fact]
        public async Task AddToCartExistingAsync()
        {
            int numberBeforeAdd = 0;
            var cartItemBeforeAdd = Data.CartItems.JohnIphone32;
            int itemVariantId = cartItemBeforeAdd.ItemVariantId;
            numberBeforeAdd = cartItemBeforeAdd.Number;
            int q = 10;
            int expected = q + numberBeforeAdd;
            await Service.AddToCartAsync(Users.JohnId, itemVariantId, q);
            int actual = (await GetQueryable().SingleAsync(c => c.Id == cartItemBeforeAdd.Id)).Number;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AddToCartNotExistingAsync()
        {
            int itemVariantId = Data.ItemVariants.ShoesXMuchFashion.Id;
            int numberBeforeAdd = 0;
            int q = 10;
            int expected = q + numberBeforeAdd;
            await Service.AddToCartAsync(Users.JohnId, itemVariantId, q);
            int actual = (await GetQueryable().SingleAsync(c => c.ItemVariantId == itemVariantId && c.OwnerId == Users.JohnId)).Number;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task RemoveFromCartExistingAsync()
        {
            var cartItem = Data.CartItems.JohnIphone64;
            int cartId = cartItem.Id;
            int itemVariantId = cartItem.ItemVariantId;
            var beforeRemove = await GetQueryable().Where(i => i.OwnerId == Users.JohnId).ToListAsync();
            int numberBeforeRemove = 0;
            numberBeforeRemove = cartItem.Number;
            int q = numberBeforeRemove - 1;
            int numberAfterRemove = numberBeforeRemove - q;
            //Assert.True(Context != Data.Context);
            await Service.RemoveFromCartAsync(Users.JohnId, itemVariantId, q);
            var updated = await GetQueryable().FirstAsync(i => i == cartItem);
            Assert.Equal(numberAfterRemove, updated.Number);
            await Service.RemoveFromCartAsync(Users.JohnId, itemVariantId, 2);
            Assert.False(await GetQueryable().AnyAsync(c => c == cartItem));
        }

        [Fact]
        public async Task RemoveFromCartNotExistingAsync()
        {
            int itemVariantId = Data.ItemVariants.ShoesXMuchFashion.Id;
            int numberBeforeAdd = 0;
            int q = 10;
            int expected = q + numberBeforeAdd;
            await Service.RemoveFromCartAsync(Users.JohnId, itemVariantId, q);
            Assert.False(await GetQueryable().AnyAsync(c => c.ItemVariantId == itemVariantId && c.OwnerId == Users.JohnId));
        }

        protected override IEnumerable<CartItem> GetCorrectNewEntites()
        {
            return new List<CartItem>
            {
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = Data.ItemVariants.JacketBlack.Id, Number = 7 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = Data.ItemVariants.Pebble1000mAh.Id, Number = 8 }
            };
        }

        protected override IEnumerable<CartItem> GetIncorrectNewEntites()
        {
            //await _context.AddAsync(new CartItem() { OwnerId = adminId, ItemVariantId = 1, Number = 7 });
            //await _context.AddAsync(new CartItem() { OwnerId = adminId, ItemVariantId = 2, Number = 8 });
            //await _context.SaveChangesAsync();
            return new List<CartItem>
            {
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = Data.ItemVariants.JacketBlack.Id, Number = 0 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = 999, Number = 8 },
                new CartItem() { OwnerId = "", ItemVariantId = 999, Number = 8 }
            };
        }

        [Fact]
        public async Task CreateAsyncThrowsAlreadyExists()
        {
            foreach (var duplicateEntity in await GetDuplicateEntitiesAsync())
            {
                await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => Service.CreateAsync(duplicateEntity));
            }
        }

        protected async Task<IEnumerable<CartItem>> GetDuplicateEntitiesAsync()
        {
            return await Task.FromResult(new List<CartItem>
            {
                // first two already exist
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = Data.ItemVariants.IPhone632GB.Id, Number = 7 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = Data.ItemVariants.IPhone664GB.Id, Number = 8 },
            });
        }

        protected override IEnumerable<CartItem> GetCorrectEntitesForUpdate()
        {
            Data.CartItems.JohnIphone32.Number = 77;
            Data.CartItems.JohnIphone64.Number = 88;
            return new List<CartItem>
            {
                Data.CartItems.JohnIphone32,
                Data.CartItems.JohnIphone64
            };
        }

        protected override IEnumerable<CartItem> GetIncorrectEntitesForUpdate()
        {
            Data.CartItems.JohnIphone32.Number = 0;
            Data.CartItems.JohnIphone64.Number = -1;
            Data.CartItems.JenniferIphone32.ItemVariantId = Data.ItemVariants.JacketBlack.Id;
            return new List<CartItem>
            {
                Data.CartItems.JohnIphone32,
                Data.CartItems.JohnIphone64,
                Data.CartItems.JenniferIphone32
            };
        }

        protected override Specification<CartItem> GetEntitiesToDeleteSpecification()
        {
            return new Specification<CartItem>(c => c.OwnerId == Data.CartItems.JohnIphone32.OwnerId);
        }
    }
}
