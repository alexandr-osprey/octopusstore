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

        protected ItemVariants ItemVariants { get; }


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
                new ItemVariantImage("IPhoneXR64GBWhite1", ItemImageContentType, ItemVariants.IPhoneXR64GBWhite.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "IPhoneXR64GBWhite1.jpg"),  },
                new ItemVariantImage("IPhoneXR64GBWhite2", ItemImageContentType, ItemVariants.IPhoneXR64GBWhite.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "IPhoneXR64GBWhite2.jpg") },
                new ItemVariantImage("IPhoneXR64GBWhite3", ItemImageContentType, ItemVariants.IPhoneXR64GBWhite.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "IPhoneXR64GBWhite3.jpg") },
                new ItemVariantImage("IPhoneXR64GBWhite4", ItemImageContentType, ItemVariants.IPhoneXR64GBWhite.Id, null) { OwnerId = Users.JohnId, FullPath = Path.Combine(PathToFiles, Users.JohnId, "IPhoneXR64GBWhite4.jpg") },
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
        }
    }
}
