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
            //Service.ScopedParameters.ClaimsPrincipal = Users.JohnPrincipal;
        }

        [Fact]
        public async Task AddToCartExistingAsync()
        {
            int numberBeforeAdd = 0;
            var cartItemBeforeAdd = _data.CartItems.JohnIPhoneXR64FromJohns;
            int itemVariantId = cartItemBeforeAdd.ItemVariantId;
            numberBeforeAdd = cartItemBeforeAdd.Number;
            int q = 10;
            int expected = q + numberBeforeAdd;
            await _service.AddToCartAsync(Users.JohnId, itemVariantId, q);
            int actual = (await GetQueryable().SingleAsync(c => c.Id == cartItemBeforeAdd.Id)).Number;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task AddToCartNotExistingAsync()
        {
            int itemVariantId = _data.ItemVariants.DanielePatriciClutch1.Id;
            int numberBeforeAdd = 0;
            int q = 10;
            int expected = q + numberBeforeAdd;
            await _service.AddToCartAsync(Users.JohnId, itemVariantId, q);
            int actual = (await GetQueryable().SingleAsync(c => c.ItemVariantId == itemVariantId && c.OwnerId == Users.JohnId)).Number;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task RemoveFromCartExistingAsync()
        {
            var cartItem = _data.CartItems.JohnIPhoneXR64FromJohns;
            int cartId = cartItem.Id;
            int itemVariantId = cartItem.ItemVariantId;
            var beforeRemove = await GetQueryable().Where(i => i.OwnerId == Users.JohnId).ToListAsync();
            int numberBeforeRemove = 0;
            numberBeforeRemove = cartItem.Number;
            int q = numberBeforeRemove - 1;
            int numberAfterRemove = numberBeforeRemove - q;
            //Assert.True(Context != Data.Context);
            await _service.RemoveFromCartAsync(Users.JohnId, itemVariantId, q);
            var updated = await GetQueryable().FirstAsync(i => i == cartItem);
            Assert.Equal(numberAfterRemove, updated.Number);
            await _service.RemoveFromCartAsync(Users.JohnId, itemVariantId, 2);
            Assert.False(await GetQueryable().AnyAsync(c => c == cartItem));
        }

        [Fact]
        public async Task RemoveFromCartNotExistingAsync()
        {
            int itemVariantId = _data.ItemVariants.SamsungGalaxyWatchWhite.Id;
            int numberBeforeAdd = 0;
            int q = 10;
            int expected = q + numberBeforeAdd;
            await _service.RemoveFromCartAsync(Users.JohnId, itemVariantId, q);
            Assert.False(await GetQueryable().AnyAsync(c => c.ItemVariantId == itemVariantId && c.OwnerId == Users.JohnId));
        }

        protected override IEnumerable<CartItem> GetCorrectNewEntites()
        {
            return new List<CartItem>
            {
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = _data.ItemVariants.SamsungGalaxyWatchWhite.Id, Number = 7 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = _data.ItemVariants.UCBDress1YellowM.Id, Number = 8 }
            };
        }

        protected override IEnumerable<CartItem> GetIncorrectNewEntites()
        {
            //await _context.AddAsync(new CartItem() { OwnerId = adminId, ItemVariantId = 1, Number = 7 });
            //await _context.AddAsync(new CartItem() { OwnerId = adminId, ItemVariantId = 2, Number = 8 });
            //await _context.SaveChangesAsync();
            return new List<CartItem>
            {
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = _data.ItemVariants.UCBDress1YellowM.Id, Number = 0 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = 999, Number = 8 },
                new CartItem() { OwnerId = "", ItemVariantId = 999, Number = 8 }
            };
        }

        [Fact]
        public async Task CreateAsyncThrowsAlreadyExists()
        {
            foreach (var duplicateEntity in await GetDuplicateEntitiesAsync())
            {
                await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.CreateAsync(duplicateEntity));
            }
        }

        protected async Task<IEnumerable<CartItem>> GetDuplicateEntitiesAsync()
        {
            return await Task.FromResult(new List<CartItem>
            {
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = _data.CartItems.JenniferIPhoneXR64FromJohns.ItemVariantId, Number = 7 },
                new CartItem() { OwnerId = Users.JohnId, ItemVariantId = _data.CartItems.JohnIPhoneXR64FromJohns.ItemVariantId, Number = 8 },
            });
        }

        protected override IEnumerable<CartItem> GetCorrectEntitesForUpdate()
        {
            _data.CartItems.JohnIPhoneXR64FromJohns.Number = 77;
            _data.CartItems.User1Dress1FromJennifers.Number = 88;
            return new List<CartItem>
            {
                _data.CartItems.JohnIPhoneXR64FromJohns,
                _data.CartItems.User1Dress1FromJennifers
            };
        }

        protected override IEnumerable<CartItem> GetIncorrectEntitesForUpdate()
        {
            _data.CartItems.JohnIPhoneXR64FromJohns.Number = 0;
            _data.CartItems.User1Dress1FromJennifers.Number = -1;
            _data.CartItems.JenniferIPhoneXR64FromJohns.ItemVariantId = _data.ItemVariants.UCBDress1YellowS.Id;
            return new List<CartItem>
            {
                _data.CartItems.JohnIPhoneXR64FromJohns,
                _data.CartItems.User1Dress1FromJennifers,
                _data.CartItems.JenniferIPhoneXR64FromJohns
            };
        }

        protected override Specification<CartItem> GetEntitiesToDeleteSpecification()
        {
            return new Specification<CartItem>(c => c.OwnerId == _data.CartItems.JohnIPhoneXR64FromJohns.OwnerId);
        }
    }
}
