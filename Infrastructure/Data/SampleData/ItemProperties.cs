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

        public ItemProperty IPhoneXR64GBWhiteBattery3000 { get; protected set; }
        public ItemProperty IPhoneXR64GBWhiteRAM3 { get; protected set; }
        public ItemProperty ReebokFastTempoWhite35White { get; protected set; }

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
                // IPhone XR
                new ItemProperty(ItemVariants.IPhoneXR64GBWhite.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBWhite.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhoneXR64GBRed.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBRed.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR64GBRed.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhoneXR128GBWhite.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBWhite.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhoneXR128GBRed.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBRed.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhoneXR128GBRed.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // iphone 8 plus
                new ItemProperty(ItemVariants.IPhone8Plus64GBWhite.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBWhite.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhone8Plus64GBBlack.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBBlack.Id, CharacteristicValues.SmartphoneColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBBlack.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBBlack.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBBlack.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhone8Plus128GBBlack.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBBlack.Id, CharacteristicValues.SmartphoneColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBBlack.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBBlack.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBBlack.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // samsung galaxy s9
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBBlack.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBBlack.Id, CharacteristicValues.SmartphoneColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBBlack.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBBlack.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBBlack.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungGalaxyS964GBRed.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBRed.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS964GBRed.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBBlack.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBBlack.Id, CharacteristicValues.SmartphoneColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBBlack.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBBlack.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBBlack.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBRed.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBRed.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS9128GBRed.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // samsung galaxy s 10
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBWhite.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBWhite.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBWhite.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBBlack.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBBlack.Id, CharacteristicValues.SmartphoneColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBBlack.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBBlack.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS1064GBBlack.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBWhite.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBWhite.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBWhite.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBBlack.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBBlack.Id, CharacteristicValues.SmartphoneColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBBlack.Id, CharacteristicValues.SmartphoneRAM8.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBBlack.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyS10128GBBlack.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // apple watch series 4
                new ItemProperty(ItemVariants.AppleWatchSeries4White.Id, CharacteristicValues.SmartwatchColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.AppleWatchSeries4Black.Id, CharacteristicValues.SmartwatchColorBlack.Id) { OwnerId = Users.JohnId },

                // samsung galaxy watch
                new ItemProperty(ItemVariants.SamsungGalaxyWatchWhite.Id, CharacteristicValues.SmartwatchColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyWatchBlack.Id, CharacteristicValues.SmartwatchColorBlack.Id) { OwnerId = Users.JohnId },

                // reebok fast tempo
                new ItemProperty(ItemVariants.ReebokFastTempoWhite35.Id, CharacteristicValues.WomensFootwearColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoWhite35.Id, CharacteristicValues.WomensFootwearSize35.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoWhite42.Id, CharacteristicValues.WomensFootwearColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoWhite42.Id, CharacteristicValues.WomensFootwearSize42.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.ReebokFastTempoBlack35.Id, CharacteristicValues.WomensFootwearColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoBlack35.Id, CharacteristicValues.WomensFootwearSize35.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoBlack42.Id, CharacteristicValues.WomensFootwearColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoBlack42.Id, CharacteristicValues.WomensFootwearSize42.Id) { OwnerId = Users.JohnId },

                // reebok DMX Run 10
                new ItemProperty(ItemVariants.ReebokDMXRun10White35.Id, CharacteristicValues.WomensFootwearColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokDMXRun10White35.Id, CharacteristicValues.WomensFootwearSize35.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokDMXRun10White42.Id, CharacteristicValues.WomensFootwearColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokDMXRun10White42.Id, CharacteristicValues.WomensFootwearSize42.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.ReebokDMXRun10Black35.Id, CharacteristicValues.WomensFootwearColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokDMXRun10Black35.Id, CharacteristicValues.WomensFootwearSize35.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokDMXRun10Black42.Id, CharacteristicValues.WomensFootwearColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokDMXRun10Black42.Id, CharacteristicValues.WomensFootwearSize42.Id) { OwnerId = Users.JohnId },

                // mark O polo shoes
                new ItemProperty(ItemVariants.MarcOPoloShoes2Black35.Id, CharacteristicValues.WomensFootwearColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.MarcOPoloShoes2Black35.Id, CharacteristicValues.WomensFootwearSize35.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.MarcOPoloShoes2Black42.Id, CharacteristicValues.WomensFootwearColorBlack.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.MarcOPoloShoes2Black42.Id, CharacteristicValues.WomensFootwearSize42.Id) { OwnerId = Users.JohnId },

                // UCB Dress 1
                new ItemProperty(ItemVariants.UCBDress1WhiteS.Id, CharacteristicValues.WomensDressSizeS.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress1WhiteS.Id, CharacteristicValues.WomensDressColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress1WhiteM.Id, CharacteristicValues.WomensDressSizeM.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress1WhiteM.Id, CharacteristicValues.WomensDressColorWhite.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.UCBDress1YellowS.Id, CharacteristicValues.WomensDressSizeS.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress1YellowS.Id, CharacteristicValues.WomensDressColorYellow.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress1YellowM.Id, CharacteristicValues.WomensDressSizeM.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress1YellowM.Id, CharacteristicValues.WomensDressColorYellow.Id) { OwnerId = Users.JohnId },

                // UCB Dress 1
                new ItemProperty(ItemVariants.UCBDress2WhiteS.Id, CharacteristicValues.WomensDressSizeS.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress2WhiteS.Id, CharacteristicValues.WomensDressColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress2WhiteM.Id, CharacteristicValues.WomensDressSizeM.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress2WhiteM.Id, CharacteristicValues.WomensDressColorWhite.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.UCBDress2YellowS.Id, CharacteristicValues.WomensDressSizeS.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress2YellowS.Id, CharacteristicValues.WomensDressColorYellow.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress2YellowM.Id, CharacteristicValues.WomensDressSizeM.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.UCBDress2YellowM.Id, CharacteristicValues.WomensDressColorYellow.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.DanielePatriciBag1.Id, CharacteristicValues.WomensAccessoryTypeBag.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.DanielePatriciBag2.Id, CharacteristicValues.WomensAccessoryTypeBag.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.DanielePatriciClutch1.Id, CharacteristicValues.WomensAccessoryTypeClutch.Id) { OwnerId = Users.JohnId },
            };
        }

        protected override IQueryable<ItemProperty> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.CharacteristicValue).Include(i => i.ItemVariant);
        }

        public override void Init()
        {
            base.Init();
            IPhoneXR64GBWhiteBattery3000 = Entities.FirstOrDefault(i => i.CharacteristicValue == CharacteristicValues.SmartphoneBattery3000 && i.ItemVariant == ItemVariants.IPhoneXR64GBWhite);
            IPhoneXR64GBWhiteRAM3 = Entities.FirstOrDefault(i => i.CharacteristicValue == CharacteristicValues.SmartphoneRAM3 && i.ItemVariant == ItemVariants.IPhoneXR64GBWhite);
            ReebokFastTempoWhite35White = Entities.FirstOrDefault(i => i.CharacteristicValue == CharacteristicValues.WomensFootwearColorWhite && i.ItemVariant == ItemVariants.ReebokDMXRun10White35);
        }
    }
}
