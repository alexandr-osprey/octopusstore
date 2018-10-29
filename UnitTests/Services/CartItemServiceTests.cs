using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CartItemServiceTests : ServiceTests<CartItem, ICartItemService>
    {
        protected IQueryable<ItemVariant> itemVariants;
        public CartItemServiceTests(ITestOutputHelper output)
           : base(output)
        {
            itemVariants = _context.Set<ItemVariant>().AsNoTracking();
        }
        [Fact]
        public async Task AddToCartAsync()
        {
            var itemVariant = await itemVariants.FirstOrDefaultAsync();
            var beforeAdd = await GetQueryable().Where(i => i.OwnerId == johnId).ToListAsync();
            int numberBeforeAdd = 0;
            var cartItemBeforeAdd = beforeAdd.FirstOrDefault(i => i.ItemVariantId == itemVariant.Id);
            if (cartItemBeforeAdd != null)
                numberBeforeAdd = cartItemBeforeAdd.Number;
            int q = 1;
            int expected = q + numberBeforeAdd;
            await _service.AddToCartAsync(johnId, itemVariant.Id, q);
            var afterAdd = await GetQueryable().Where(i => i.OwnerId == johnId).ToListAsync();
            int actual = (await GetQueryable().FirstOrDefaultAsync(i => i.OwnerId == johnId && i.ItemVariantId == itemVariant.Id)).Number;
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task RemoveFromCartAsync()
        {
            var itemVariant = await itemVariants.FirstOrDefaultAsync();
            var beforeRemove = await GetQueryable().Where(i => i.OwnerId == johnId).ToListAsync();
            int numberBeforeRemove = 0;
            var cartItemBeforeRemove = beforeRemove.FirstOrDefault(i => i.ItemVariantId == itemVariant.Id);
            if (cartItemBeforeRemove != null)
                numberBeforeRemove = cartItemBeforeRemove.Number;
            int q = 1;
            int numberAfterRemoveExpected = numberBeforeRemove - q;
            numberAfterRemoveExpected = numberAfterRemoveExpected < 0 ? 0 : numberAfterRemoveExpected;
            await _service.RemoveFromCartAsync(johnId, itemVariant.Id, q);
            var afterRemove = await GetQueryable().Where(i => i.OwnerId == johnId).ToListAsync();
            if (numberAfterRemoveExpected <= 0)
                Assert.False(await GetQueryable().Where(i => i.OwnerId == johnId && i.ItemVariantId == itemVariant.Id).AnyAsync());
            else
                Assert.Equal(numberAfterRemoveExpected, (await GetQueryable().FirstOrDefaultAsync(i => i.OwnerId == johnId && i.ItemVariantId == itemVariant.Id)).Number);
        }

        protected override async Task<IEnumerable<CartItem>> GetCorrectNewEntitesAsync()
        {
            return await Task.FromResult(new List<CartItem>
            {
                new CartItem() { OwnerId = johnId, ItemVariantId = 2, Number = 7 },
                new CartItem() { OwnerId = johnId, ItemVariantId = 3, Number = 8 }
            });
        }

        protected override async Task<IEnumerable<CartItem>> GetIncorrectNewEntitesAsync()
        {
            //await _context.AddAsync(new CartItem() { OwnerId = adminId, ItemVariantId = 1, Number = 7 });
            //await _context.AddAsync(new CartItem() { OwnerId = adminId, ItemVariantId = 2, Number = 8 });
            //await _context.SaveChangesAsync();
            return await Task.FromResult(new List<CartItem>
            {
                new CartItem() { OwnerId = johnId, ItemVariantId = 3, Number = 0 },
                new CartItem() { OwnerId = johnId, ItemVariantId = 999, Number = 8 },
                new CartItem() { OwnerId = "", ItemVariantId = 999, Number = 8 }
            });
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
                // first two already exist
                new CartItem() { OwnerId = johnId, ItemVariantId = 1, Number = 7 },
                new CartItem() { OwnerId = johnId, ItemVariantId = 2, Number = 8 },
            });
        }

        protected override async Task<IEnumerable<CartItem>> GetCorrectEntitesForUpdateAsync()
        {
            return await Task.FromResult(new List<CartItem>
            {
                new CartItem() { Id = 1, OwnerId = johnId, ItemVariantId = 1, Number = 77 },
                new CartItem() { Id = 2, OwnerId = johnId, ItemVariantId = 2, Number = 88 },
            });
        }

        protected override async Task<IEnumerable<CartItem>> GetIncorrectEntitesForUpdateAsync()
        {
            return await Task.FromResult(new List<CartItem>
            {
                new CartItem() { Id = 1, OwnerId = johnId, ItemVariantId = 1, Number = -1 },
                //new CartItem() { Id = 2, OwnerId = johnId, ItemVariantId = 99, Number = 88 },
            });
        }

        protected override Specification<CartItem> GetEntitiesToDeleteSpecification()
        {
            return new Specification<CartItem>(c => c.ItemVariantId == 1 && c.OwnerId == johnId);
        }
    }
}
