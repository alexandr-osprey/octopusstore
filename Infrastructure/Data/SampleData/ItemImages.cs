using ApplicationCore.Entities;
using System.Collections.Generic;
using System.IO;

namespace Infrastructure.Data.SampleData
{
    public class ItemImages: SampleDataEntities<ItemImage>
    {
        public static string ItemImageContentType { get; } = @"image/jpeg";
        public static string PathToFiles { get; } = @"C:\files\";
        public static string PathToBackup { get; } = @"C:\files\backup";

        public Items Items { get; }

        public ItemImage IPhone61 { get; }
        public ItemImage IPhone62 { get; }
        public ItemImage IPhone63 { get; }
        public ItemImage Samsung71 { get; }
        public ItemImage Samsung81 { get; }
        public ItemImage Pebble1 { get; }
        public ItemImage Shoes1 { get; }
        public ItemImage Jacket1 { get; }

        public ItemImages(StoreContext storeContext, Items items): base(storeContext)
        {
            Items = items;
            Seed();

            IPhone61 = Entities[0];
            IPhone62 = Entities[1];
            IPhone63 = Entities[2];
            Samsung71 = Entities[3];
            Samsung81 = Entities[4];
            Pebble1 = Entities[5];
            Shoes1 = Entities[6];
            Jacket1 = Entities[7];
        }

        protected override IEnumerable<ItemImage> GetSourceEntities()
        {
            return new List<ItemImage>
            {
                new ItemImage("iPhone 6 - 1", ItemImageContentType, Items.IPhone6.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 1.jpg"),  },
                new ItemImage("iPhone 6 - 2.jpg", ItemImageContentType, Items.IPhone6.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 2.jpg") },
                new ItemImage("iPhone 6 - 3.jpg", ItemImageContentType, Items.IPhone6.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 3.jpg") },
                new ItemImage("Samsung 7.jpg", ItemImageContentType, Items.Samsung7.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 7.jpg") },
                new ItemImage("Samsung 8.jpg", ItemImageContentType, Items.Samsung8.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 8.jpg") },
                new ItemImage("Pebble.jpg", ItemImageContentType, Items.PebbleWatch.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Pebble.jpg") },

                new ItemImage("Shoes.jpg", ItemImageContentType, Items.Shoes.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Shoes.jpg") },
                new ItemImage("Jacket.jpg", ItemImageContentType, Items.Jacket.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Jacket.jpg") }
            };
        }

        protected override void AfterSeed(List<ItemImage> entities)
        {
            EnsureFilesAreInPlace(entities);
            base.AfterSeed(entities);
        }

        public void EnsureFilesAreInPlace(List<ItemImage> entities)
        {
            foreach (var itemImage in entities)
            {
                if (!File.Exists(itemImage.FullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(itemImage.FullPath));
                    string rel = Path.GetRelativePath(PathToFiles, itemImage.FullPath);
                    string fullBackupPath = Path.Combine(PathToBackup, rel);
                    File.Copy(fullBackupPath, itemImage.FullPath);
                }
            }
        }
    }
}
