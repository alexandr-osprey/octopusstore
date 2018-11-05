using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class ItemVariantServiceTests : ServiceTests<ItemVariant, IItemVariantService>
    {
        public ItemVariantServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override IEnumerable<ItemVariant> GetCorrectEntitesForUpdate()
        {
            var itemVariant = Data.ItemVariants.IPhone664GB;
            itemVariant.Title = "Updated";
            itemVariant.Price = 999;
            return new List<ItemVariant>()
            {
                itemVariant
            };
        }

        protected override IEnumerable<ItemVariant> GetCorrectNewEntites()
        {
            return new List<ItemVariant>()
            {
                new ItemVariant() { ItemId = Data.Items.IPhone6.Id, Title = "title 1", Price = 500 },
            };
        }

        [Fact]
        public async Task DeleteSingleWithRelatedRelinkAsync()
        {
            var entity = Data.ItemVariants.IPhone664GB;
            int idToRelinkTo = Data.ItemVariants.IPhone632GB.Id;
            var itemProperties = Data.ItemProperties.Entities.Where(i => i.ItemVariant == entity).ToList();
            await Service.DeleteSingleWithRelatedRelink(entity.Id, idToRelinkTo);
            itemProperties.ForEach(i => Assert.Equal(i.ItemVariantId, idToRelinkTo));
            Assert.False(Context.Set<ItemVariant>().Any(c => c == entity));
        }

        protected override Specification<ItemVariant> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemVariant>(i => i.Title.Contains("Samsung"));
        }

        protected override IEnumerable<ItemVariant> GetIncorrectEntitesForUpdate()
        {
            var itemVariant = Data.ItemVariants.JacketBlack;
            return new List<ItemVariant>()
            {
                new ItemVariant() { Id = itemVariant.Id, Title = "", ItemId = itemVariant.ItemId, OwnerId = itemVariant.OwnerId, Price = itemVariant.Price },
                new ItemVariant() { Id = itemVariant.Id, Title = itemVariant.Title, ItemId = itemVariant.ItemId, OwnerId = itemVariant.OwnerId, Price = 0 },
            };
        }

        protected override IEnumerable<ItemVariant> GetIncorrectNewEntites()
        {
            var itemVariant = Data.ItemVariants.JacketBlack;
            return new List<ItemVariant>()
            {
                new ItemVariant() { ItemId = 0, Title = "title 1", Price = 500 },
                new ItemVariant() { ItemId = itemVariant.ItemId, Title = null, Price = 100 },
                new ItemVariant() { ItemId = itemVariant.ItemId, Title = "title 3", Price = 0 },
            };
        }

        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.ItemProperties);
        }
    }
}
