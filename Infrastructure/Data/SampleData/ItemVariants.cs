using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class ItemVariants: SampleDataEntities<ItemVariant>
    {
        protected Items Items { get; }

        public ItemVariant IPhoneXR64GBWhite  { get; protected set; }
        public ItemVariant IPhoneXR64GBRed { get; protected set; }
        public ItemVariant IPhoneXR128GBWhite { get; protected set; }
        public ItemVariant IPhoneXR128GBRed { get; protected set; }

        public ItemVariant IPhone8Plus64GBWhite { get; protected set; }
        public ItemVariant IPhone8Plus64GBRed { get; protected set; }
        public ItemVariant IPhone8Plus128GBWhite { get; protected set; }
        public ItemVariant IPhone8Plus128GBRed { get; protected set; }

        public ItemVariant SamsungS964GBWhite { get; protected set; }
        public ItemVariant SamsungS964GBRed { get; protected set; }
        public ItemVariant SamsungS9128GBWhite { get; protected set; }
        public ItemVariant SamsungS9128GBRed { get; protected set; }

        public ItemVariant SamsungS1064GBWhite { get; protected set; }
        public ItemVariant SamsungS1064GBRed { get; protected set; }
        public ItemVariant SamsungS10128GBWhite { get; protected set; }
        public ItemVariant SamsungS10128GBRed { get; protected set; }

        public ItemVariant AppleWatchSeries4White { get; protected set; }
        public ItemVariant AppleWatchSeries4Black { get; protected set; }

        public ItemVariant SamsungGalaxyWatchWhite { get; protected set; }
        public ItemVariant SamsungGalaxyWatchBlack { get; protected set; }

        public ItemVariant ReebokFastTempoWhite35 { get; protected set; }
        public ItemVariant ReebokFastTempoWhite42 { get; protected set; }
        public ItemVariant ReebokFastTempoBlack35 { get; protected set; }
        public ItemVariant ReebokFastTempoBlack42 { get; protected set; }

        public ItemVariant ReebokDMXRun10White35 { get; protected set; }
        public ItemVariant ReebokDMXRun10White42 { get; protected set; }
        public ItemVariant ReebokDMXRun10Black35 { get; protected set; }
        public ItemVariant ReebokDMXRun10Black42 { get; protected set; }

        public ItemVariant MarcOPoloShoes1White35 { get; protected set; }
        public ItemVariant MarcOPoloShoes1White42 { get; protected set; }
        public ItemVariant MarcOPoloShoes1Black35 { get; protected set; }
        public ItemVariant MarcOPoloShoes1Black42 { get; protected set; }

        public ItemVariant MarcOPoloShoes2White35 { get; protected set; }
        public ItemVariant MarcOPoloShoes2White42 { get; protected set; }
        public ItemVariant MarcOPoloShoes2Black35 { get; protected set; }
        public ItemVariant MarcOPoloShoes2Black42 { get; protected set; }

        public ItemVariant UCBDress1WhiteS { get; protected set; }
        public ItemVariant UCBDress1WhiteM { get; protected set; }
        public ItemVariant UCBDress1YellowS { get; protected set; }
        public ItemVariant UCBDress1YellowM { get; protected set; }

        public ItemVariant UCBDress2YellowS { get; protected set; }
        public ItemVariant UCBDress2YellowM { get; protected set; }
        public ItemVariant UCBDress2WhiteS { get; protected set; }
        public ItemVariant UCBDress2WhiteM { get; protected set; }

        public ItemVariant DanielePatriciBag1 { get; protected set; }

        public ItemVariant DanielePatriciBag2 { get; protected set; }

        public ItemVariant DanielePatriciClutch1 { get; protected set; }

        public ItemVariants(StoreContext storeContext, Items items): base(storeContext)
        {
            Items = items;
            Seed();
            Init();
        }

        protected override IEnumerable<ItemVariant> GetSourceEntities()
        {
            return new List<ItemVariant>
            {
                new ItemVariant { Title = "IPhone XR 64GB White", Price = 1000, ItemId = Items.IPhoneXR.Id, OwnerId = Users.JohnId }, //0
                new ItemVariant { Title = "IPhone XR 64GB Red", Price = 1000, ItemId = Items.IPhoneXR.Id, OwnerId = Users.JohnId }, //1
                new ItemVariant { Title = "IPhone XR 128GB White", Price = 1200, ItemId = Items.IPhoneXR.Id, OwnerId = Users.JohnId }, //2
                new ItemVariant { Title = "IPhone XR 128GB Red", Price = 1300, ItemId = Items.IPhoneXR.Id, OwnerId = Users.JohnId }, //3

                new ItemVariant { Title = "IPhone 8 Plus 64GB White", Price = 900, ItemId = Items.IPhone8Plus.Id, OwnerId = Users.JohnId }, //4
                new ItemVariant { Title = "IPhone 8 Plus 64GB Red", Price = 900, ItemId = Items.IPhone8Plus.Id, OwnerId = Users.JohnId }, //5
                new ItemVariant { Title = "IPhone 8 Plus 128GB White", Price = 1000, ItemId = Items.IPhone8Plus.Id, OwnerId = Users.JohnId }, //6
                new ItemVariant { Title = "IPhone 8 Plus 128GB Red", Price = 1000, ItemId = Items.IPhone8Plus.Id, OwnerId = Users.JohnId }, //7

                new ItemVariant { Title = "Samsung Galaxy S9 64GB White", Price = 900, ItemId = Items.SamsungS9.Id, OwnerId = Users.JohnId }, //8
                new ItemVariant { Title = "Samsung Galaxy S9 64GB Red", Price = 900, ItemId = Items.SamsungS9.Id, OwnerId = Users.JohnId }, //9
                new ItemVariant { Title = "Samsung Galaxy S9 128GB White", Price = 1100, ItemId = Items.SamsungS9.Id, OwnerId = Users.JohnId }, //10
                new ItemVariant { Title = "Samsung Galaxy S9 128GB Red", Price = 1150, ItemId = Items.SamsungS9.Id, OwnerId = Users.JohnId }, //11

                new ItemVariant { Title = "Samsung Galaxy S10 64GB White", Price = 1000, ItemId = Items.SamsungS10.Id, OwnerId = Users.JohnId }, //12
                new ItemVariant { Title = "Samsung Galaxy S10 64GB Red", Price = 1000, ItemId = Items.SamsungS10.Id, OwnerId = Users.JohnId }, //13
                new ItemVariant { Title = "Samsung Galaxy S10 128GB White", Price = 1100, ItemId = Items.SamsungS10.Id, OwnerId = Users.JohnId }, //14
                new ItemVariant { Title = "Samsung Galaxy S10 128GB Red", Price = 1200, ItemId = Items.SamsungS10.Id, OwnerId = Users.JohnId }, //15

                new ItemVariant { Title = "Apple Watch Series 4 White", Price = 700, ItemId = Items.AppleWatchSeries4.Id, OwnerId = Users.JohnId }, //16
                new ItemVariant { Title = "Apple Watch Series 4 Black", Price = 700, ItemId = Items.AppleWatchSeries4.Id, OwnerId = Users.JohnId }, //17

                new ItemVariant { Title = "Samsung Galaxy Watch White", Price = 600, ItemId = Items.AppleWatchSeries4.Id, OwnerId = Users.JohnId }, //18
                new ItemVariant { Title = "Samsung Galaxy Watch Black", Price = 600, ItemId = Items.AppleWatchSeries4.Id, OwnerId = Users.JohnId }, //19

                new ItemVariant { Title = "Reebok Fast Tempo White 35", Price = 500, ItemId = Items.ReebokFastTempo.Id, OwnerId = Users.JenniferId }, //20
                new ItemVariant { Title = "Reebok Fast Tempo White 42", Price = 500, ItemId = Items.ReebokFastTempo.Id, OwnerId = Users.JenniferId }, //21
                new ItemVariant { Title = "Reebok Fast Tempo Black 35", Price = 500, ItemId = Items.ReebokFastTempo.Id, OwnerId = Users.JenniferId }, //22
                new ItemVariant { Title = "Reebok Fast Tempo Black 42", Price = 500, ItemId = Items.ReebokFastTempo.Id, OwnerId = Users.JenniferId }, //23

                new ItemVariant { Title = "Reebok DMX Run 10 White 35", Price = 500, ItemId = Items.ReebokDMXRun10.Id, OwnerId = Users.JenniferId }, //24
                new ItemVariant { Title = "Reebok DMX Run 10 White 42", Price = 500, ItemId = Items.ReebokDMXRun10.Id, OwnerId = Users.JenniferId }, //25
                new ItemVariant { Title = "Reebok DMX Run 10 Black 35", Price = 500, ItemId = Items.ReebokDMXRun10.Id, OwnerId = Users.JenniferId }, //26
                new ItemVariant { Title = "Reebok DMX Run 10 Black 42", Price = 500, ItemId = Items.ReebokDMXRun10.Id, OwnerId = Users.JenniferId }, //27

                new ItemVariant { Title = "Mark O' Polo Shoes White 35", Price = 500, ItemId = Items.MarcOPoloShoes1.Id, OwnerId = Users.JenniferId }, //28
                new ItemVariant { Title = "Mark O' Polo Shoes White 42", Price = 500, ItemId = Items.MarcOPoloShoes1.Id, OwnerId = Users.JenniferId }, //29
                new ItemVariant { Title = "Mark O' Polo Shoes Black 35", Price = 500, ItemId = Items.MarcOPoloShoes1.Id, OwnerId = Users.JenniferId }, //30
                new ItemVariant { Title = "Mark O' Polo Shoes Black 42", Price = 500, ItemId = Items.MarcOPoloShoes1.Id, OwnerId = Users.JenniferId }, //31

                new ItemVariant { Title = "Mark O' Polo White 35", Price = 500, ItemId = Items.MarcOPoloShoes2.Id, OwnerId = Users.JenniferId }, //32
                new ItemVariant { Title = "Mark O' Polo White 42", Price = 500, ItemId = Items.MarcOPoloShoes2.Id, OwnerId = Users.JenniferId }, //33
                new ItemVariant { Title = "Mark O' Polo Black 35", Price = 500, ItemId = Items.MarcOPoloShoes2.Id, OwnerId = Users.JenniferId }, //34
                new ItemVariant { Title = "Mark O' Polo Black 42", Price = 500, ItemId = Items.MarcOPoloShoes2.Id, OwnerId = Users.JenniferId }, //35

                new ItemVariant { Title = "United Colors Of Benetton White S", Price = 500, ItemId = Items.UCBDress1.Id, OwnerId = Users.JenniferId }, //36
                new ItemVariant { Title = "United Colors Of Benetton White M", Price = 500, ItemId = Items.UCBDress1.Id, OwnerId = Users.JenniferId }, //37
                new ItemVariant { Title = "United Colors Of Benetton Yellow S", Price = 500, ItemId = Items.UCBDress1.Id, OwnerId = Users.JenniferId }, //38
                new ItemVariant { Title = "United Colors Of Benetton Yellow M", Price = 500, ItemId = Items.UCBDress1.Id, OwnerId = Users.JenniferId }, //39

                new ItemVariant { Title = "United Colors Of Benetton White S", Price = 700, ItemId = Items.UCBDress2.Id, OwnerId = Users.JenniferId }, //40
                new ItemVariant { Title = "United Colors Of Benetton White M", Price = 700, ItemId = Items.UCBDress2.Id, OwnerId = Users.JenniferId }, //41
                new ItemVariant { Title = "United Colors Of Benetton Yellow S", Price = 600, ItemId = Items.UCBDress2.Id, OwnerId = Users.JenniferId }, //42
                new ItemVariant { Title = "United Colors Of Benetton Yellow M", Price = 600, ItemId = Items.UCBDress2.Id, OwnerId = Users.JenniferId }, //43

                new ItemVariant { Title = "Daniele Patrici", Price = 500, ItemId = Items.DanielePatriciBag1.Id, OwnerId = Users.JenniferId }, //44

                new ItemVariant { Title = "Daniele Patrici", Price = 500, ItemId = Items.DanielePatriciBag2.Id, OwnerId = Users.JenniferId }, //45

                new ItemVariant { Title = "Daniele Patrici", Price = 500, ItemId = Items.DanielePatriciClutch1.Id, OwnerId = Users.JenniferId }, //46
            };
        }

        protected override IQueryable<ItemVariant> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.Item).Include(i => i.ItemProperties);
        }

        public override void Init()
        {
            base.Init();
            IPhoneXR64GBWhite = Entities[0];
            IPhoneXR64GBRed = Entities[1];
            IPhoneXR128GBWhite = Entities[2];
            IPhoneXR128GBRed = Entities[3];

            IPhone8Plus64GBWhite = Entities[4];
            IPhone8Plus64GBRed = Entities[5];
            IPhone8Plus128GBWhite = Entities[6];
            IPhone8Plus128GBRed = Entities[7];

            SamsungS964GBWhite = Entities[8];
            SamsungS964GBRed = Entities[9];
            SamsungS9128GBWhite = Entities[10];
            SamsungS9128GBRed = Entities[11];

            SamsungS1064GBWhite = Entities[12];
            SamsungS1064GBRed = Entities[13];
            SamsungS10128GBWhite = Entities[14];
            SamsungS10128GBRed = Entities[15];

            AppleWatchSeries4White = Entities[16];
            AppleWatchSeries4Black = Entities[17];

            SamsungGalaxyWatchWhite = Entities[18];
            SamsungGalaxyWatchBlack = Entities[19];

            ReebokFastTempoWhite35 = Entities[20];
            ReebokFastTempoWhite42 = Entities[21];
            ReebokFastTempoBlack35 = Entities[22];
            ReebokFastTempoBlack42 = Entities[23];

            ReebokDMXRun10White35 = Entities[24];
            ReebokDMXRun10White42 = Entities[25];
            ReebokDMXRun10Black35 = Entities[26];
            ReebokDMXRun10Black42 = Entities[27];

            MarcOPoloShoes1White35 = Entities[28];
            MarcOPoloShoes1White42 = Entities[29];
            MarcOPoloShoes1Black35 = Entities[30];
            MarcOPoloShoes1Black42 = Entities[31];

            MarcOPoloShoes2White35 = Entities[32];
            MarcOPoloShoes2White42 = Entities[33];
            MarcOPoloShoes2Black35 = Entities[34];
            MarcOPoloShoes2Black42 = Entities[35];

            UCBDress1WhiteS = Entities[36];
            UCBDress1WhiteM = Entities[37];
            UCBDress1YellowS = Entities[38];
            UCBDress1YellowM = Entities[39];

            UCBDress2WhiteS = Entities[40];
            UCBDress2WhiteM = Entities[41];
            UCBDress2YellowS = Entities[42];
            UCBDress2YellowM = Entities[43];

            DanielePatriciBag1 = Entities[44];

            DanielePatriciBag2 = Entities[45];

            DanielePatriciClutch1 = Entities[46];
        }
    }
}
