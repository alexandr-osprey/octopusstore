using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Interfaces.Controllers;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    public class ItemVariantControllerTest : ControllerTests<ItemVariant, ItemVariantViewModel, IItemVariantsController, IItemVariantService>
    {
        public ItemVariantControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexByItemAsync()
        {
            int itemId = Data.Items.IPhone6.Id;
            var variants = Data.ItemVariants.Entities.Where(i => i.ItemId == itemId);
            var expected = new IndexViewModel<ItemVariantViewModel>(1, 1, variants.Count(), from v in variants select new ItemVariantViewModel(v));
            var actual = await Controller.IndexAsync(itemId);
            Equal(expected, actual);
        }

        [Fact]
        public async Task GetDetailAsync()
        {
            var expected = new ItemVariantDetailViewModel(GetQueryable().Where(i => i == Data.ItemVariants.IPhone664GB).First());
            var actual = await Controller.ReadDetailAsync(expected.Id);
            Equal(expected, actual);
        }

        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return Context
                    .Set<ItemVariant>()
                    
                    .Include(j => j.ItemProperties);
        }

        protected override void AssertUpdateSuccess(ItemVariant beforeUpdate, ItemVariantViewModel expected, ItemVariantViewModel actual)
        {
            Assert.Equal(beforeUpdate.ItemId, actual.ItemId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Price, actual.Price);
        }

        protected override IEnumerable<ItemVariant> GetCorrectEntitiesToCreate()
        {
            return new List<ItemVariant>() { new ItemVariant() { ItemId = Data.Items.Jacket.Id, Price = 505, Title = "title" } };
        }

        protected override ItemVariantViewModel ToViewModel(ItemVariant entity)
        {
            return new ItemVariantViewModel()
            {
                Id = entity.Id,
                ItemId = entity.ItemId,
                Price = entity.Price,
                Title = entity.Title
            };
        }

        protected override IEnumerable<ItemVariantViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<ItemVariantViewModel>()
            {
                new ItemVariantViewModel()
                {
                    Id = Data.ItemVariants.IPhone664GB.Id,
                    ItemId = 999,
                    Price = 500,
                    Title = "UPD"
                }
            };
        }
    }
}
