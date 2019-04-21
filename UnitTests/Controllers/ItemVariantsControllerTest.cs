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
    public class ItemVariantsControllerTest : ControllerTests<ItemVariant, ItemVariantViewModel, IItemVariantsController, IItemVariantService>
    {
        public ItemVariantsControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task IndexByItemAsync()
        {
            int itemId = _data.Items.IPhoneXR.Id;
            var variants = _data.ItemVariants.Entities.Where(i => i.ItemId == itemId);
            var expected = new IndexViewModel<ItemVariantViewModel>(1, 1, variants.Count(), from v in variants select new ItemVariantViewModel(v));
            var actual = await _controller.IndexAsync(itemId);
            Equal(expected, actual);
        }

        [Fact]
        public async Task GetDetailAsync()
        {
            var expected = new ItemVariantDetailViewModel(GetQueryable().Where(i => i == _data.ItemVariants.IPhoneXR128GBRed).First());
            var actual = await _controller.ReadDetailAsync(expected.Id);
            Equal(expected, actual);
        }

        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return _context
                    .Set<ItemVariant>()
                    
                    .Include(j => j.ItemProperties);
        }

        protected override Task AssertUpdateSuccessAsync(ItemVariant beforeUpdate, ItemVariantViewModel expected, ItemVariantViewModel actual)
        {
            Assert.Equal(beforeUpdate.ItemId, actual.ItemId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Price, actual.Price);
            return Task.CompletedTask;
        }

        protected override IEnumerable<ItemVariant> GetCorrectEntitiesToCreate()
        {
            return new List<ItemVariant>() { new ItemVariant() { ItemId = _data.Items.MarcOPoloShoes1.Id, Price = 505, Title = "title" } };
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
                    Id = _data.ItemVariants.IPhoneXR64GBRed.Id,
                    ItemId = 999,
                    Price = 500,
                    Title = "UPD"
                }
            };
        }
    }
}
