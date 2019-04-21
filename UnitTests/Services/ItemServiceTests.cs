using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Infrastructure.Data;
using ApplicationCore.Interfaces.Services;

namespace UnitTests.Services
{
    public class ItemServiceTests : ServiceTests<Item, IItemService>
    {
        public ItemServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<Item> GetCorrectEntitesForUpdate()
        {
            var item = _data.Items.IPhone8Plus;
            item.Title = "Updated title";
            item.Description = "Updated Description";
            return new List<Item>() { item };
        }

        protected override IEnumerable<Item> GetCorrectNewEntites()
        {
            return new List<Item>()
            {
                new Item()
                {
                    BrandId = _data.Brands.MarcOPolo.Id,
                    CategoryId = _data.Categories.WomensFootwear.Id,
                    StoreId = _data.Stores.Jennifers.Id,
                    Description = "Desc",
                    Title = "title",
                }
            };
        }

        protected override async Task AssertRelatedDeleted(Item entity)
        {
            Assert.False(await _context.Set<ItemVariantImage>().AnyAsync(i => i.RelatedId == entity.Id));
        }
        protected override Specification<Item> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Item>(i => i.Title.Contains("iPhone"));
        }

        protected override IEnumerable<Item> GetIncorrectEntitesForUpdate()
        {
            _data.Items.AppleWatchSeries4.Title = null;
            _data.Items.IPhoneXR.BrandId = _data.Brands.MarcOPolo.Id;
            _data.Items.SamsungS10.StoreId = _data.Stores.Jennifers.Id;
            _data.Items.SamsungS9.CategoryId = _data.Categories.WomensFootwear.Id;
            _data.Items.IPhone8Plus.Description = "";

            return new List<Item>()
            {
                _data.Items.AppleWatchSeries4,
                _data.Items.IPhoneXR,
                _data.Items.SamsungS10,
                _data.Items.SamsungS10,
                _data.Items.SamsungS9,
                _data.Items.IPhone8Plus
            };
        }

        [Fact]
        public async Task EnumerateByPriceAsync()
        {
            var spec = new Specification<Item>();
            spec.OrderByExpressions.Add(i => (from v in i.ItemVariants select v.Price).Min());
            var actual = await _service.EnumerateAsync(spec);
            var expected = _context.Set<Item>()
                .AsNoTracking()
                .OrderBy(i => (from v in i.ItemVariants select v.Price).Min());
            Equal(expected, actual);
        }

        protected override IEnumerable<Item> GetIncorrectNewEntites()
        {
            var item = _data.Items.SamsungS9;
            return new List<Item>()
            {
                new Item()
                {
                    Title = null,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = 99,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = 1,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = 99,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = null,
                    OwnerId = item.OwnerId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    StoreId = 999
                },
            };
        }
    }
}
