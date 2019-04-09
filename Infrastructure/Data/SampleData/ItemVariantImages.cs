using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class ItemVariantImages : SampleDataEntities<ItemVariantImage>
    {
        public static string ItemImageContentType { get; } = @"image/jpeg";
        public static string PathToFiles { get; } = @"C:\files\";
        public static string PathToBackup { get; } = @"C:\files\backup";

        protected ItemVariants ItemVariants { get; }

        public ItemVariantImage IPhone61 { get; protected set; }
        public ItemVariantImage IPhone62 { get; protected set; }
        public ItemVariantImage IPhone63 { get; protected set; }
        public ItemVariantImage Samsung71 { get; protected set; }
        public ItemVariantImage Samsung81 { get; protected set; }
        public ItemVariantImage Pebble1 { get; protected set; }
        public ItemVariantImage Shoes1 { get; protected set; }
        public ItemVariantImage Jacket1 { get; protected set; }

        public ItemVariantImages(StoreContext storeContext, ItemVariants itemVariants) : base(storeContext)
        {
            ItemVariants = itemVariants;
            Seed();
            Init();
        }

        protected override IEnumerable<ItemVariantImage> GetSourceEntities()
        {
            var list = new List<ItemVariantImage>
            {
                new ItemVariantImage("iPhone_6___1", ItemImageContentType, ItemVariants.Entities[0].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 1.jpg"),  },
                new ItemVariantImage("iPhone_6___2", ItemImageContentType, ItemVariants.Entities[0].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 2.jpg") },
                new ItemVariantImage("iPhone_6___3", ItemImageContentType, ItemVariants.Entities[1].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "iPhone 6 - 3.jpg") },
                new ItemVariantImage("Samsung_7", ItemImageContentType, ItemVariants.Entities[2].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 7.jpg") },
                new ItemVariantImage("Samsung_7", ItemImageContentType, ItemVariants.Entities[3].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 7.jpg") },
                new ItemVariantImage("Samsung_8", ItemImageContentType, ItemVariants.Entities[4].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 8.jpg") },
                new ItemVariantImage("Samsung_8", ItemImageContentType, ItemVariants.Entities[5].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Samsung 8.jpg") },
                new ItemVariantImage("Pebble", ItemImageContentType, ItemVariants.Entities[6].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "Pebble.jpg") },

                new ItemVariantImage("Shoes", ItemImageContentType, ItemVariants.Entities[7].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Shoes.jpg") },
                new ItemVariantImage("Shoes", ItemImageContentType, ItemVariants.Entities[8].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Shoes.jpg") },
                new ItemVariantImage("Jacket", ItemImageContentType, ItemVariants.Entities[9].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Jacket.jpg") },
                new ItemVariantImage("Jacket", ItemImageContentType, ItemVariants.Entities[10].Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JenniferId, "Jacket.jpg") }
            };
            return list;
        }

        protected override void AfterSeed(IEnumerable<ItemVariantImage> entities)
        {
            EnsureFilesAreInPlace(entities);
            base.AfterSeed(entities);
        }

        public void EnsureFilesAreInPlace(IEnumerable<ItemVariantImage> entities)
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

        protected override IQueryable<ItemVariantImage> GetQueryable()
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
