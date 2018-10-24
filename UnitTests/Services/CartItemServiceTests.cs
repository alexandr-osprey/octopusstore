using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class CartItemServiceTests : ServiceTestBase<CartItem, ICartItemService>
    {
        protected IQueryable<ItemVariant> itemVariants;
        public CartItemServiceTests(ITestOutputHelper output)
            : base(output)
        {
            itemVariants = context.Set<ItemVariant>().AsNoTracking();
        }
        public async Task PopulateData()
        {
            var itemVariant = await itemVariants.FirstOrDefaultAsync();
            await context.AddAsync(new CartItem() { OwnerId = johnId, ItemVariantId = itemVariant.Id, Number = 5 });
            await context.AddAsync(new CartItem() { OwnerId = johnId, ItemVariantId = itemVariant.Id + 1, Number = 6 });
            await context.SaveChangesAsync();
        }
        [Fact]
        public async Task AddToCartWhenExistsAsync()
        {
            await PopulateData();
            await AddToCartAsync();
        }
        [Fact]
        public async Task AddToCartAsync()
        {
            var itemVariant = await itemVariants.FirstOrDefaultAsync();
            var beforeAdd = await GetQueryable(context).Where(i => i.OwnerId == johnId).ToListAsync();
            int numberBeforeAdd = 0;
            var cartItemBeforeAdd = beforeAdd.FirstOrDefault(i => i.ItemVariantId == itemVariant.Id);
            if (cartItemBeforeAdd != null)
                numberBeforeAdd = cartItemBeforeAdd.Number;
            int q = 1;
            int numberAfterAdd = q + numberBeforeAdd;
            await service.AddToCartAsync(itemVariant.Id, q);
            var afterAdd = await GetQueryable(context).Where(i => i.OwnerId == johnId).ToListAsync();
            Assert.Equal(numberAfterAdd, (await GetQueryable(context).FirstOrDefaultAsync(i => i.OwnerId == johnId && i.ItemVariantId == itemVariant.Id)).Number);
        }
        [Fact]
        public async Task EnumerateCartItemsAsync()
        {
            await PopulateData();
            var expected = await GetQueryable(context).Where(i => i.OwnerId == johnId).ToListAsync();
            var actual = await service.EnumerateCartItemsAsync();
            Equal(expected, actual);
        }
        [Fact]
        public async Task RemoveFromCartWhenExistsAsync()
        {
            await PopulateData();
            await RemoveFromCartAsync();
        }
        [Fact]
        public async Task RemoveFromCartAsync()
        {
            var itemVariant = await itemVariants.FirstOrDefaultAsync();
            var beforeRemove = await GetQueryable(context).Where(i => i.OwnerId == johnId).ToListAsync();
            int numberBeforeRemove = 0;
            var cartItemBeforeRemove = beforeRemove.FirstOrDefault(i => i.ItemVariantId == itemVariant.Id);
            if (cartItemBeforeRemove != null)
                numberBeforeRemove = cartItemBeforeRemove.Number;
            int q = 1;
            int numberAfterRemoveExpected = numberBeforeRemove - q;
            numberAfterRemoveExpected = numberAfterRemoveExpected < 0 ? 0 : numberAfterRemoveExpected;
            await service.RemoveFromCartAsync(itemVariant.Id, q);
            var afterRemove = await GetQueryable(context).Where(i => i.OwnerId == johnId).ToListAsync();
            if (numberAfterRemoveExpected <= 0)
                Assert.False(await GetQueryable(context).Where(i => i.OwnerId == johnId && i.ItemVariantId == itemVariant.Id).AnyAsync());
            else
                Assert.Equal(numberAfterRemoveExpected, (await GetQueryable(context).FirstOrDefaultAsync(i => i.OwnerId == johnId && i.ItemVariantId == itemVariant.Id)).Number);
        }
    }
}
