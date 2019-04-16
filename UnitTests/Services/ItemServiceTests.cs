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
            var item = Data.Items.Jacket;
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
                    BrandId = Data.Brands.CalvinKlein.Id,
                    CategoryId = Data.Categories.WomensFootwear.Id,
                    MeasurementUnitId = Data.MeasurementUnits.Pcs.Id,
                    StoreId = Data.Stores.Jennifers.Id,
                    Description = "Desc",
                    Title = "title",
                }
            };
        }

        protected override async Task AssertRelatedDeleted(Item entity)
        {
            Assert.False(await Context.Set<ItemVariantImage>().AnyAsync(i => i.RelatedId == entity.Id));
        }
        protected override Specification<Item> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Item>(i => i.Title.Contains("iPhone"));
        }

        protected override IEnumerable<Item> GetIncorrectEntitesForUpdate()
        {
            Data.Items.AppleWatchSeries4.Title = null;
            Data.Items.IPhoneXR.BrandId = Data.Brands.Pebble.Id;
            Data.Items.Jacket.MeasurementUnitId = Data.MeasurementUnits.M.Id;
            Data.Items.SamsungS10.StoreId = Data.Stores.Jennifers.Id;
            Data.Items.SamsungS9.CategoryId = Data.Categories.WomensFootwear.Id;
            Data.Items.Shoes.Description = "";

            return new List<Item>()
            {
                Data.Items.AppleWatchSeries4,
                Data.Items.IPhoneXR,
                Data.Items.Jacket,
                Data.Items.SamsungS10,
                Data.Items.SamsungS9,
                Data.Items.Shoes
            };
        }

        [Fact]
        public async Task DeleteSingleWithRelatedRelinkAsync()
        {
            var entity = Data.Items.SamsungS10;
            int idToRelinkTo = Data.Items.SamsungS9.Id;
            var itemImages = Data.ItemImages.Entities.Where(i => i.RelatedEntity == entity).ToList();
            var itemVariants = Data.ItemVariants.Entities.Where(i => i.Item == entity).ToList();
            await Service.DeleteSingleWithRelatedRelink(entity.Id, idToRelinkTo);
            itemImages.ForEach(i => Assert.Equal(i.RelatedId, idToRelinkTo));
            itemVariants.ForEach(i => Assert.Equal(i.ItemId, idToRelinkTo));
            Assert.False(Context.Set<Item>().Any(c => c == entity));
        }

        [Fact]
        public async Task EnumerateByPriceAsync()
        {
            var spec = new Specification<Item>();
            spec.OrderByExpressions.Add(i => (from v in i.ItemVariants select v.Price).Min());
            var actual = await Service.EnumerateAsync(spec);
            var expected = Context.Set<Item>()
                .AsNoTracking()
                .OrderBy(i => (from v in i.ItemVariants select v.Price).Min());
            Equal(expected, actual);
        }

        protected override IEnumerable<Item> GetIncorrectNewEntites()
        {
            var item = Data.Items.SamsungS9;
            return new List<Item>()
            {
                new Item()
                {
                    Title = null,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = 99,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = 1,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = 99,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = null,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = 99,
                    StoreId = item.StoreId
                },
                new Item()
                {
                    Title = item.Title,
                    BrandId = item.BrandId,
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    OwnerId = item.OwnerId,
                    MeasurementUnitId = item.MeasurementUnitId,
                    StoreId = 99
                },
            };
        }

        protected override IQueryable<Item> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.Images)
                    .Include(i => i.ItemVariants)
                        .ThenInclude(i => i.ItemProperties);
        }
    }
}
