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

                new ItemProperty(ItemVariants.IPhone8Plus64GBRed.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBRed.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus64GBRed.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBWhite.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.IPhone8Plus128GBRed.Id, CharacteristicValues.SmartphoneBattery3000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBRed.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.IPhone8Plus128GBRed.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // samsung galaxy s9
                new ItemProperty(ItemVariants.SamsungS964GBWhite.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBWhite.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungS964GBRed.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBRed.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS964GBRed.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungS9128GBWhite.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBWhite.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBWhite.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungS9128GBRed.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBRed.Id, CharacteristicValues.SmartphoneRAM3.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHD.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS9128GBRed.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // samsung galaxy s 10
                new ItemProperty(ItemVariants.SamsungS1064GBWhite.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBWhite.Id, CharacteristicValues.SmartphoneRAM4.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBWhite.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungS1064GBRed.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBRed.Id, CharacteristicValues.SmartphoneRAM4.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS1064GBRed.Id, CharacteristicValues.SmartphoneStorage64GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungS10128GBWhite.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBWhite.Id, CharacteristicValues.SmartphoneColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBWhite.Id, CharacteristicValues.SmartphoneRAM4.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBWhite.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBWhite.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                new ItemProperty(ItemVariants.SamsungS10128GBRed.Id, CharacteristicValues.SmartphoneBattery4000.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBRed.Id, CharacteristicValues.SmartphoneColorRed.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBRed.Id, CharacteristicValues.SmartphoneRAM4.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBRed.Id, CharacteristicValues.SmartphoneResolutionFullHDPlus.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungS10128GBRed.Id, CharacteristicValues.SmartphoneStorage128GB.Id) { OwnerId = Users.JohnId },

                // apple watch series 4
                new ItemProperty(ItemVariants.AppleWatchSeries4White.Id, CharacteristicValues.SmartwatchColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.AppleWatchSeries4Black.Id, CharacteristicValues.SmartwatchColorBlack.Id) { OwnerId = Users.JohnId },

                // samsung galaxy watch
                new ItemProperty(ItemVariants.SamsungGalaxyWatchWhite.Id, CharacteristicValues.SmartwatchColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.SamsungGalaxyWatchBlack.Id, CharacteristicValues.SmartwatchColorBlack.Id) { OwnerId = Users.JohnId },

                // reebok fast tempo
                new ItemProperty(ItemVariants.ReebokFastTempoWhite.Id, CharacteristicValues.WomensFootwearColorWhite.Id) { OwnerId = Users.JohnId },
                new ItemProperty(ItemVariants.ReebokFastTempoWhite.Id, CharacteristicValues.WomensFootwearSize35.Id) { OwnerId = Users.JohnId },
            };
        }

        protected override IQueryable<ItemProperty> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.CharacteristicValue).Include(i => i.ItemVariant);
        }

        public override void Init()
        {
            base.Init();
        }
    }
}
