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
            var item = _data.Items.Jacket;
            item.Title = "Updated title";
            item.Description = "Updated Description";
            return new List<Item>()
            {
                item
            };
        }

        protected override IEnumerable<Item> GetCorrectNewEntites()
        {
            return new List<Item>()
            {
                new Item()
                {
                    BrandId = _data.Brands.CK.Id,
                    CategoryId = _data.Categories.Shoes.Id,
                    MeasurementUnitId = _data.MeasurementUnits.Pcs.Id,
                    StoreId = _data.Stores.Jennifers.Id,
                    Description = "Desc",
                    Title = "title",
                }
            };
        }

        protected override async Task AssertRelatedDeleted(Item entity)
        {
            Assert.False(await _context.Set<ItemImage>().AnyAsync(i => i.RelatedId == entity.Id));
        }
        protected override Specification<Item> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Item>(i => i.Title.Contains("iPhone"));
        }

        protected override IEnumerable<Item> GetIncorrectEntitesForUpdate()
        {
            _data.Items.PebbleWatch.Title = null;
            return new List<Item>()
            {
                _data.Items.PebbleWatch
            };
        }

        protected override IEnumerable<Item> GetIncorrectNewEntites()
        {
            var item = _data.Items.Samsung8;
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
