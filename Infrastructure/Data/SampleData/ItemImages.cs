using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class ItemImages : SampleDataEntities<ItemImage>
    {
        public static string ItemImageContentType { get; } = @"image/jpeg";
        public static string PathToFiles { get; } = @"C:\files\";
        public static string PathToBackup { get; } = @"C:\files\backup";

        protected Items Items { get; }

        public ItemImage IPhone61 { get; protected set; }
        public ItemImage IPhone62 { get; protected set; }
        public ItemImage IPhone63 { get; protected set; }
        public ItemImage Samsung71 { get; protected set; }
        public ItemImage Samsung81 { get; protected set; }
        public ItemImage Pebble1 { get; protected set; }
        public ItemImage Shoes1 { get; protected set; }
        public ItemImage Jacket1 { get; protected set; }

        public ItemImages(StoreContext storeContext, Items items) : base(storeContext)
        {
            Items = items;
            Seed();
            Init();
        }

        protected override IEnumerable<ItemImage> GetSourceEntities()
        {
            var list = new List<ItemImage>();
            int l = 6;
            for (int i = 0; i < Items.Entities.Count; i += l)
            {
                list.AddRange(
                    new List<ItemImage>
                    {
                        new ItemImage("iPhone_6___1", ItemImageContentType, Items.Entities[i].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 1.jpg"),  },
                        new ItemImage("iPhone_6___2", ItemImageContentType, Items.Entities[i].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 2.jpg") },
                        new ItemImage("iPhone_6___3", ItemImageContentType, Items.Entities[i].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 3.jpg") },
                        new ItemImage("Samsung_7", ItemImageContentType, Items.Entities[i + 1].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 7.jpg") },
                        new ItemImage("Samsung_8", ItemImageContentType, Items.Entities[i + 2].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 8.jpg") },
                        new ItemImage("Pebble", ItemImageContentType, Items.Entities[i + 3].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Pebble.jpg") },

                        new ItemImage("Shoes", ItemImageContentType, Items.Entities[i + 4].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Shoes.jpg") },
                        new ItemImage("Jacket", ItemImageContentType, Items.Entities[i + 5].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Jacket.jpg") }
                    });
            };
            return list;
        }

        protected override void AfterSeed(IEnumerable<ItemImage> entities)
        {
            EnsureFilesAreInPlace(entities);
            base.AfterSeed(entities);
        }

        public void EnsureFilesAreInPlace(IEnumerable<ItemImage> entities)
        {
            foreach (var itemImage in entities)
            {
                if (!File.Exists(itemImage.FullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(itemImage.FullPath));
                    string rel = Path.GetRelativePath(PathToFiles, itemImage.FullPath);
                    var bytes = GetItemImageByteArray(itemImage.Title);
                    if (bytes == null)
                        continue;
                    File.WriteAllBytes(itemImage.FullPath, bytes);
                }
            }
        }

        protected byte[] GetItemImageByteArray(string imageNameWithoutExtension)
        {
            return (byte[])typeof(Properties.Resources).GetProperty(imageNameWithoutExtension).GetValue(null);
        }

        protected override IQueryable<ItemImage> GetQueryable()
        {
            return base.GetQueryable().Include(i => i.RelatedEntity);
        }

        public override void Init()
        {
            base.Init();
            IPhone61 = Entities[0];
            IPhone62 = Entities[1];
            IPhone63 = Entities[2];
            Samsung71 = Entities[3];
            Samsung81 = Entities[4];
            Pebble1 = Entities[5];
            Shoes1 = Entities[6];
            Jacket1 = Entities[7];
        }
    }
}
