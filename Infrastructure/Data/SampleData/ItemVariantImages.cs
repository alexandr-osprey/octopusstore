using ApplicationCore.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Data.SampleData
{
    public class ItemVariantImages : SampleDataEntities<ItemVariantImage>
    {
        private string ItemVariantImageContentType { get; } = @"image/jpeg";
        private string PathToFiles { get; }

        protected ItemVariants ItemVariants { get; }
        protected Categories Categories { get; }


        public ItemVariantImages(StoreContext storeContext, ItemVariants itemVariants, Categories categories, IConfiguration configuration) : base(storeContext)
        {
            ItemVariants = itemVariants;
            Categories = categories;
            PathToFiles = Path.Combine(configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "files");
            Seed();
            Init();
        }

        protected override IEnumerable<ItemVariantImage> GetSourceEntities()
        {
            var list = new List<ItemVariantImage>();
            list.AddRange(CreateIndexedEntities(Categories.Smartphones, Users.JohnId, 4));
            list.AddRange(CreateIndexedEntities(Categories.Smartwatches, Users.JohnId, 2));
            list.AddRange(CreateIndexedEntities(Categories.WomensFootwear, Users.JenniferId, 2));
            list.AddRange(CreateIndexedEntities(Categories.WomensDresses, Users.JenniferId, 2));
            list.AddRange(CreateIndexedEntities(Categories.WomensAccesories, Users.JenniferId, 1));
            return list;
        }

        protected IEnumerable<ItemVariantImage> CreateIndexedEntities(Category category, string ownerId, int maxIndex)
        {
            var list = new List<ItemVariantImage>();
            var variants = ItemVariants.Entities.Where(v => v.Item.Category == category);
            foreach (var variant in variants)
            {
                for (int i = 1; i < maxIndex + 1; i++)
                {
                    string t = variant.Title.Replace(" ", string.Empty).Replace("'", string.Empty) + i.ToString();
                    list.Add(new ItemVariantImage(t, ItemVariantImageContentType, variant.Id, null, null) { OwnerId = ownerId, FullPath = Path.Combine(PathToFiles, ownerId, $"{t}.jpg"), });
                }
            }
            return list;
        }

        protected override void AfterSeed(IEnumerable<ItemVariantImage> entities)
        {
            EnsureFilesAreInPlace(entities);
            base.AfterSeed(entities);
        }

        public void EnsureFilesAreInPlace(IEnumerable<ItemVariantImage> entities)
        {
            foreach (var itemVariantImage in entities)
            {
                if (!File.Exists(itemVariantImage.FullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(itemVariantImage.FullPath));
                    string rel = Path.GetRelativePath(PathToFiles, itemVariantImage.FullPath);
                    var bytes = GetItemVariantImageByteArray(itemVariantImage.Title);
                    if (bytes == null)
                        continue;
                    File.WriteAllBytes(itemVariantImage.FullPath, bytes);
                }
            }
        }

        protected byte[] GetItemVariantImageByteArray(string imageNameWithoutExtension)
        {
            return (byte[])typeof(Properties.Resources).GetProperty(imageNameWithoutExtension)?.GetValue(null) ?? throw new System.Exception($"Image {imageNameWithoutExtension} not found");
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
