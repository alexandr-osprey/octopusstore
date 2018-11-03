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

        protected override async Task<IEnumerable<Item>> GetCorrectEntitesForUpdateAsync()
        {
            var firstItem = await GetQueryable().FirstOrDefaultAsync();
            firstItem.Title = "Updated title";
            firstItem.Description = "Updated Description";
            return new List<Item>()
            {
                firstItem
            };
        }

        protected override async Task<IEnumerable<Item>> GetCorrectNewEntitesAsync()
        {
            var firstItem = await _context.Set<Item>().FirstOrDefaultAsync();
            firstItem.Id = 0;
            firstItem.Description = "desc1";
            return new List<Item>()
            {
                firstItem
            };
        }

        protected override async Task AfterDeleteAsync(Item entity)
        {
            await base.AfterDeleteAsync(entity);
            //StoreContextSeed.EnsureFilesAreInPlace();
        }
        protected override async Task AssertRelatedDeleted(Item entity)
        {
            Assert.False(await _context.Set<ItemImage>().AnyAsync(i => i.RelatedId == entity.Id));
        }
        protected override Specification<Item> GetEntitiesToDeleteSpecification()
        {
            return new Specification<Item>(i => i.Title.Contains("iPhone"));
        }

        protected override async Task<IEnumerable<Item>> GetIncorrectEntitesForUpdateAsync()
        {
            var firstItem = await GetQueryable().FirstOrDefaultAsync();
            firstItem.Title = null;
            return new List<Item>()
            {
                firstItem
            };
        }

        protected override async Task<IEnumerable<Item>> GetIncorrectNewEntitesAsync()
        {
            var firstItem = await GetQueryable().FirstOrDefaultAsync();
            return new List<Item>()
            {
                new Item()
                {
                    Title = null,
                    BrandId = firstItem.BrandId,
                    CategoryId = firstItem.CategoryId,
                    Description = firstItem.Description,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = firstItem.MeasurementUnitId,
                    StoreId = firstItem.StoreId
                },
                new Item()
                {
                    Title = firstItem.Title,
                    BrandId = 99,
                    CategoryId = firstItem.CategoryId,
                    Description = firstItem.Description,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = firstItem.MeasurementUnitId,
                    StoreId = firstItem.StoreId
                },
                new Item()
                {
                    Title = firstItem.Title,
                    BrandId = firstItem.BrandId,
                    CategoryId = 1,
                    Description = firstItem.Description,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = firstItem.MeasurementUnitId,
                    StoreId = firstItem.StoreId
                },
                new Item()
                {
                    Title = firstItem.Title,
                    BrandId = firstItem.BrandId,
                    CategoryId = 99,
                    Description = firstItem.Description,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = firstItem.MeasurementUnitId,
                    StoreId = firstItem.StoreId
                },
                new Item()
                {
                    Title = firstItem.Title,
                    BrandId = firstItem.BrandId,
                    CategoryId = firstItem.CategoryId,
                    Description = null,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = firstItem.MeasurementUnitId,
                    StoreId = firstItem.StoreId
                },
                new Item()
                {
                    Title = firstItem.Title,
                    BrandId = firstItem.BrandId,
                    CategoryId = firstItem.CategoryId,
                    Description = firstItem.Description,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = 99,
                    StoreId = firstItem.StoreId
                },
                new Item()
                {
                    Title = firstItem.Title,
                    BrandId = firstItem.BrandId,
                    CategoryId = firstItem.CategoryId,
                    Description = firstItem.Description,
                    OwnerId = firstItem.OwnerId,
                    MeasurementUnitId = firstItem.MeasurementUnitId,
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
