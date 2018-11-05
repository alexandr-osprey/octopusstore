using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class ItemProperties : SampleDataEntities<ItemProperty>
    {
        protected ItemVariants ItemVariants { get; }
        protected CharacteristicValues CharacteristicValues { get; }

        public ItemProperty IPhone632GB32 { get; protected set; }
        public ItemProperty IPhone632HD { get; protected set; }
        public ItemProperty IPhone664GB32 { get; protected set; }
        public ItemProperty IPhone664HD { get; protected set; }
        public ItemProperty Samsung732HDGB32 { get; protected set; }
        public ItemProperty Samsung732HDHD { get; protected set; }
        public ItemProperty Samsung732GB32FHDGB32 { get; protected set; }
        public ItemProperty Samsung732GB32FHDFullHD { get; protected set; }
        public ItemProperty Samsung832HDGB32 { get; protected set; }
        public ItemProperty Samsung832HDHD { get; protected set; }
        public ItemProperty Samsung832GB32FHDGB32 { get; protected set; }
        public ItemProperty Samsung832GB32FHDFullHD { get; protected set; }
        public ItemProperty Pebble1000mAhMAh1000 { get; protected set; }

        public ItemProperty ShoesXMuchFashionX { get; protected set; }
        public ItemProperty ShoesXMuchFashionMuchFashion { get; protected set; }
        public ItemProperty ShoesXXLMuchFashionXXL { get; protected set; }
        public ItemProperty ShoesXXLMuchFashionMuchFashion { get; protected set; }
        public ItemProperty JacketBlackBlack { get; protected set; }
        public ItemProperty JacketWhiteWhite { get; protected set; }

        public ItemProperties(StoreContext storeContext, ItemVariants itemVariants, CharacteristicValues characteristicValues) : base(storeContext)
        {
            ItemVariants = itemVariants;
            CharacteristicValues = characteristicValues;
            Seed();
            Init();
        }

        protected override IEnumerable<ItemProperty> GetSourceEntities()
        {
            return new List<ItemProperty>
            {
                // iphone 32
                new ItemProperty(ItemVariants.IPhone632GB.Id, CharacteristicValues.GB32.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone632GB.Id, CharacteristicValues.HD.Id) { OwnerId = Users.JohnId },
                // iphone 64
                new ItemProperty(ItemVariants.IPhone664GB.Id, CharacteristicValues.GB32.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone664GB.Id, CharacteristicValues.FullHD.Id) { OwnerId = Users.JohnId },
                //samsung 7 32 hd
                new ItemProperty(ItemVariants.Samsung732GBHD.Id, CharacteristicValues.GB32.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.Samsung732GBHD.Id, CharacteristicValues.HD.Id) { OwnerId = Users.JohnId },
                //samsung 7 32 full hd
                new ItemProperty(ItemVariants.Samsung732GBFullHD.Id, CharacteristicValues.GB32.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.Samsung732GBFullHD.Id, CharacteristicValues.FullHD.Id) { OwnerId = Users.JohnId },
                //samsung 8 64 hd
                new ItemProperty(ItemVariants.Samsung832GBHD.Id, CharacteristicValues.GB32.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.Samsung832GBHD.Id, CharacteristicValues.HD.Id) { OwnerId = Users.JohnId },
                //samsung 8 64 full hd
                new ItemProperty(ItemVariants.Samsung832GBFullHD.Id, CharacteristicValues.GB32.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.Samsung832GBFullHD.Id, CharacteristicValues.FullHD.Id) { OwnerId = Users.JohnId },
                //pebble 100mAh
                new ItemProperty(ItemVariants.Pebble1000mAh.Id, CharacteristicValues.MAh1000.Id) { OwnerId = Users.JohnId },

                //shoes X much fashion
                new ItemProperty(ItemVariants.ShoesXMuchFashion.Id, CharacteristicValues.X.Id) { OwnerId = Users.JenniferId },
                new ItemProperty(ItemVariants.ShoesXMuchFashion.Id, CharacteristicValues.MuchFashion.Id) { OwnerId = Users.JenniferId },
                //shoes XXL much fashion
                new ItemProperty(ItemVariants.ShoesXXLMuchFashion.Id, CharacteristicValues.XXL.Id) { OwnerId = Users.JenniferId },
                new ItemProperty(ItemVariants.ShoesXXLMuchFashion.Id, CharacteristicValues.MuchFashion.Id) { OwnerId = Users.JenniferId },
                //jacket black
                new ItemProperty(ItemVariants.JacketBlack.Id, CharacteristicValues.Black.Id) { OwnerId = Users.JenniferId },
                //jacket white
                new ItemProperty(ItemVariants.JacketWhite.Id, CharacteristicValues.White.Id) { OwnerId = Users.JenniferId },
            };
        }

        protected override IQueryable<ItemProperty> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.CharacteristicValue).Include(i => i.ItemVariant);
        }

        public override void Init()
        {
            base.Init();
            IPhone632GB32 = Entities[0];
            IPhone632HD = Entities[1];
            IPhone664GB32 = Entities[2];
            IPhone664HD = Entities[3];
            Samsung732HDGB32 = Entities[4];
            Samsung732HDHD = Entities[5];
            Samsung732GB32FHDGB32 = Entities[6];
            Samsung732GB32FHDFullHD = Entities[7];
            Samsung832HDGB32 = Entities[8];
            Samsung832HDHD = Entities[9];
            Samsung832GB32FHDGB32 = Entities[10];
            Samsung832GB32FHDFullHD = Entities[11];
            Pebble1000mAhMAh1000 = Entities[12];
            ShoesXMuchFashionX = Entities[13];
            ShoesXMuchFashionMuchFashion = Entities[14];
            ShoesXXLMuchFashionXXL = Entities[15];
            ShoesXXLMuchFashionMuchFashion = Entities[16];
            JacketBlackBlack = Entities[17];
            JacketWhiteWhite = Entities[18];
        }
    }
}
