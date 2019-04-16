using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class CharacteristicValues: SampleDataEntities<CharacteristicValue>
    {
        protected Characteristics Characteristics { get; }
 
        public CharacteristicValue SmartphoneStorage64GB { get; protected set; }
        public CharacteristicValue SmartphoneStorage128GB { get; protected set; }

        public CharacteristicValue SmartphoneResolutionFullHD { get; protected set; }
        public CharacteristicValue SmartphoneResolutionFullHDPlus { get; protected set; }

        public CharacteristicValue SmartphoneRAM3 { get; protected set; }
        public CharacteristicValue SmartphoneRAM4 { get; protected set; }

        public CharacteristicValue SmartphoneBattery3000 { get; protected set; }
        public CharacteristicValue SmartphoneBattery4000 { get; protected set; }

        public CharacteristicValue SmartphoneColorWhite { get; protected set; }
        public CharacteristicValue SmartphoneColorBlack { get; protected set; }
        public CharacteristicValue SmartphoneColorRed { get; protected set; }

        public CharacteristicValue SmartwatchColorWhite { get; protected set; }
        public CharacteristicValue SmartwatchColorBlack { get; protected set; }
        public CharacteristicValue SmartwatchColorRed { get; protected set; }

        public CharacteristicValue WomensFootwearSize35 { get; protected set; }
        public CharacteristicValue WomensFootwearSize42 { get; protected set; }

        public CharacteristicValue WomensFootwearTypeSneakers { get; protected set; }
        public CharacteristicValue WomensFootwearTypeShoes { get; protected set; }

        public CharacteristicValue WomensFootwearColorBlack { get; protected set; }
        public CharacteristicValue WomensFootwearColorWhite { get; protected set; }

        public CharacteristicValue WomensDressColorYellow { get; protected set; }
        public CharacteristicValue WomensDressColorWhite { get; protected set; }

        public CharacteristicValue WomensDressSizeS { get; protected set; }
        public CharacteristicValue WomensDressSizeM { get; protected set; }

        public CharacteristicValue WomensAccessoryTypeBag { get; protected set; }
        public CharacteristicValue WomensAccessoryTypeClutch { get; protected set; }

        public CharacteristicValue WomensAccessoryColorBlack { get; protected set; }
        public CharacteristicValue WomensAccessoryColorWhite { get; protected set; }

        public CharacteristicValues(StoreContext storeContext, Characteristics characteristics) : base(storeContext)
        {
            Characteristics = characteristics;
            Seed();
            Init();
        }

        protected override IEnumerable<CharacteristicValue> GetSourceEntities()
        {
            return new List<CharacteristicValue>
            {
                new CharacteristicValue { Title = "64GB", CharacteristicId = Characteristics.SmartphoneStorage.Id, OwnerId = Users.AdminId }, //0
                new CharacteristicValue { Title = "128GB", CharacteristicId = Characteristics.SmartphoneStorage.Id, OwnerId = Users.AdminId }, //1

                new CharacteristicValue { Title = "Full HD", CharacteristicId = Characteristics.SmartphoneResolution.Id, OwnerId = Users.AdminId }, //2
                new CharacteristicValue { Title = "Full HD Plus", CharacteristicId = Characteristics.SmartphoneResolution.Id, OwnerId = Users.AdminId }, //3

                new CharacteristicValue { Title = "3GB", CharacteristicId = Characteristics.SmartphoneRAM.Id, OwnerId = Users.AdminId }, //4
                new CharacteristicValue { Title = "4GB", CharacteristicId = Characteristics.SmartphoneRAM.Id, OwnerId = Users.AdminId }, //5

                new CharacteristicValue { Title = "3000 mAh", CharacteristicId = Characteristics.SmartphoneBattery.Id, OwnerId = Users.AdminId }, //6
                new CharacteristicValue { Title = "4000 mAh", CharacteristicId = Characteristics.SmartphoneBattery.Id, OwnerId = Users.AdminId }, //7

                new CharacteristicValue { Title = "White", CharacteristicId = Characteristics.SmartphoneColor.Id, OwnerId = Users.AdminId }, //8
                new CharacteristicValue { Title = "Black", CharacteristicId = Characteristics.SmartphoneColor.Id, OwnerId = Users.AdminId }, //9
                new CharacteristicValue { Title = "Red", CharacteristicId = Characteristics.SmartphoneColor.Id, OwnerId = Users.AdminId }, //10

                new CharacteristicValue { Title = "White", CharacteristicId = Characteristics.SmartwatchColor.Id, OwnerId = Users.AdminId }, //11
                new CharacteristicValue { Title = "Black", CharacteristicId = Characteristics.SmartwatchColor.Id, OwnerId = Users.AdminId }, //12
                new CharacteristicValue { Title = "Red", CharacteristicId = Characteristics.SmartwatchColor.Id, OwnerId = Users.AdminId }, //13

                new CharacteristicValue { Title = "35", CharacteristicId = Characteristics.WomenFootwearSize.Id, OwnerId = Users.AdminId }, //14
                new CharacteristicValue { Title = "42", CharacteristicId = Characteristics.WomenFootwearSize.Id, OwnerId = Users.AdminId }, //15

                new CharacteristicValue { Title = "Sneakers", CharacteristicId = Characteristics.WomenFootwearType.Id, OwnerId = Users.AdminId }, //16
                new CharacteristicValue { Title = "Shoes", CharacteristicId = Characteristics.WomenFootwearType.Id, OwnerId = Users.AdminId }, //17

                new CharacteristicValue { Title = "Black", CharacteristicId = Characteristics.WomenFootwearColor.Id, OwnerId = Users.AdminId }, //18
                new CharacteristicValue { Title = "White", CharacteristicId = Characteristics.WomenFootwearColor.Id, OwnerId = Users.AdminId }, //19

                new CharacteristicValue { Title = "Yellow", CharacteristicId = Characteristics.WomensDressColor.Id, OwnerId = Users.AdminId }, //20
                new CharacteristicValue { Title = "White", CharacteristicId = Characteristics.WomensDressColor.Id, OwnerId = Users.AdminId }, //21

                new CharacteristicValue { Title = "S", CharacteristicId = Characteristics.WomensDressSize.Id, OwnerId = Users.AdminId }, //22
                new CharacteristicValue { Title = "M", CharacteristicId = Characteristics.WomensDressSize.Id, OwnerId = Users.AdminId }, //23

                new CharacteristicValue { Title = "Bag", CharacteristicId = Characteristics.WomensAccessoryType.Id, OwnerId = Users.AdminId }, //24
                new CharacteristicValue { Title = "Clutch", CharacteristicId = Characteristics.WomensAccessoryType.Id, OwnerId = Users.AdminId }, //25

                new CharacteristicValue { Title = "Black", CharacteristicId = Characteristics.WomensAccessoryColor.Id, OwnerId = Users.AdminId }, //26
                new CharacteristicValue { Title = "White", CharacteristicId = Characteristics.WomensAccessoryColor.Id, OwnerId = Users.AdminId }, //27
            };
        }

        protected override IQueryable<CharacteristicValue> GetQueryable()
        {
            return base.GetQueryable().Include(c => c.Characteristic).Include(c => c.ItemProperties);
        }

        public override void Init()
        {
            base.Init();
            SmartphoneStorage64GB = Entities[0];
            SmartphoneStorage128GB = Entities[1];

            SmartphoneResolutionFullHD = Entities[2];
            SmartphoneResolutionFullHDPlus = Entities[3];

            SmartphoneRAM3 = Entities[4];
            SmartphoneRAM4 = Entities[5];

            SmartphoneBattery3000 = Entities[6];
            SmartphoneBattery4000 = Entities[7];

            SmartphoneColorWhite = Entities[8];
            SmartphoneColorBlack = Entities[9];
            SmartphoneColorRed = Entities[10];

            SmartphoneColorWhite = Entities[11];
            SmartphoneColorBlack = Entities[12];
            SmartphoneColorRed = Entities[13];

            WomensFootwearSize35 = Entities[14];
            WomensFootwearSize42 = Entities[15];

            WomensFootwearTypeSneakers = Entities[16];
            WomensFootwearTypeShoes = Entities[17];

            WomensFootwearColorBlack = Entities[18];
            WomensFootwearColorWhite = Entities[19];

            WomensDressColorYellow = Entities[20];
            WomensDressColorWhite = Entities[21];

            WomensDressSizeS = Entities[22];
            WomensDressSizeM = Entities[23];

            WomensAccessoryTypeBag = Entities[24];
            WomensAccessoryTypeClutch = Entities[25];

            WomensAccessoryColorBlack = Entities[26];
            WomensAccessoryColorWhite = Entities[27];
        }
    }
}
