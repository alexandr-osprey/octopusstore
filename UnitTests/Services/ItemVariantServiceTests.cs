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
            var itemVariant = _data.ItemVariants.IPhone8Plus128GBWhite;
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
                new ItemVariant() { ItemId = _data.Items.IPhoneXR.Id, Title = "title 1", Price = 500 },
            };
        }

        protected override Specification<ItemVariant> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemVariant>(i => i.Title.Contains("Samsung"));
        }

        protected override IEnumerable<ItemVariant> GetIncorrectEntitesForUpdate()
        {
            _data.ItemVariants.MarcOPoloShoes1Black35.ItemId = _data.Items.IPhoneXR.Id;
            _data.ItemVariants.IPhone8Plus128GBBlack.Title = "";
            _data.ItemVariants.IPhone8Plus64GBWhite.Price = 0;
            return new List<ItemVariant>()
            {
                _data.ItemVariants.MarcOPoloShoes1Black35,
                _data.ItemVariants.IPhone8Plus128GBBlack,
                _data.ItemVariants.IPhone8Plus64GBWhite
            };
        }

        protected override IEnumerable<ItemVariant> GetIncorrectNewEntites()
        {
            var itemVariant = _data.ItemVariants.MarcOPoloShoes1Black35;
            return new List<ItemVariant>()
            {
                new ItemVariant() { ItemId = 0, Title = "title 1", Price = 500 },
                new ItemVariant() { ItemId = itemVariant.ItemId, Title = null, Price = 100 },
                new ItemVariant() { ItemId = itemVariant.ItemId, Title = "title 3", Price = 0 },
            };
        }

        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.ItemProperties).Include(i => i.Images);

        }
    }
}
