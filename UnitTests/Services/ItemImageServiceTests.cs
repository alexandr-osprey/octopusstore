using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class ItemImageServiceTests : ServiceTests<ItemVariantImage, IItemVariantImageService>
    {
        public ItemImageServiceTests(ITestOutputHelper output)
           : base(output)
        {
            _imageToCreateFrom = Data.ItemVariantImages.Samsung71;
        }

        protected ItemVariantImage _imageToCreateFrom;

        protected override IEnumerable<ItemVariantImage> GetCorrectNewEntites()
        {
            return new List<ItemVariantImage>()
            {
                new ItemVariantImage("testImage1", _imageToCreateFrom.ContentType, _imageToCreateFrom.RelatedId, Service.GetStream(_imageToCreateFrom))
            };
        }

        protected override IEnumerable<ItemVariantImage> GetIncorrectNewEntites()
        {
            var itemImage = Data.ItemVariantImages.IPhone63;
            return new List<ItemVariantImage>()
            {
                new ItemVariantImage("", @"image/jpg", itemImage.RelatedId, Service.GetStream(itemImage)),
                new ItemVariantImage("test", @"image/jpg", 999, Service.GetStream(itemImage)),
                new ItemVariantImage("test", @"image/jpg", itemImage.RelatedId, null),
                new ItemVariantImage("test",  @"image/exe", itemImage.RelatedId, Service.GetStream(itemImage)),
            };
        }

        //[Fact]
        protected override async Task AssertCreateSuccessAsync(ItemVariantImage itemImage)
        {
            await base.AssertCreateSuccessAsync(itemImage);
            Assert.True(Context.ItemVariantImages.Contains(itemImage));
            using (var fileDest = File.Open(itemImage.FullPath, FileMode.Open))
            {
                using (var fileInit = File.Open(_imageToCreateFrom.FullPath, FileMode.Open))
                {
                    Assert.Equal(fileInit.Length, fileDest.Length);
                }
            }
            Directory.Delete(itemImage.DirectoryPath, true);
        }

        [Fact]
        public async Task GetStream()
        {
            using (var stream = Service.GetStream(Data.ItemVariantImages.Jacket1))
            {
                byte[] readBytes = new byte[stream.Length];
                int bytesCount = await stream.ReadAsync(readBytes, 0, (int)stream.Length);
                Assert.Equal(stream.Length, bytesCount);
            }
        }
        //protected override async Task BeforeDeleteAsync(ItemImage itemImage)
        //{
        //    await base.BeforeDeleteAsync(itemImage);
        //    string fileCopy = itemImage.FullPath + "_copy";
        //    if (!File.Exists(fileCopy))
        //    {
        //        File.Copy(itemImage.FullPath, fileCopy);
        //    };
        //    itemImage.FullPath = fileCopy;
        //    _context.ItemImages.Update(itemImage);
        //    await _context.SaveChangesAsync();
        //}
        //protected override async Task AfterDeleteAsync(ItemImage itemImage)
        //{
        //    if (File.Exists(itemImage.FullPath))
        //        File.Delete(itemImage.FullPath);
        //    await Task.CompletedTask;
        //}
        protected override async Task AssertRelatedDeleted(ItemVariantImage itemImage)
        {
            await base.AssertRelatedDeleted(itemImage);
            Assert.False(File.Exists(itemImage.FullPath));
        }

        protected override IEnumerable<ItemVariantImage> GetCorrectEntitesForUpdate()
        {
            Data.ItemVariantImages.Samsung81.Title = "Updated 1";
            return new List<ItemVariantImage>() { Data.ItemVariantImages.Samsung81 };
        }

        protected override IEnumerable<ItemVariantImage> GetIncorrectEntitesForUpdate()
        {
            Data.ItemVariantImages.Samsung81.Title = "";
            Data.ItemVariantImages.IPhone61.ContentType = "txt";
            Data.ItemVariantImages.IPhone62.DirectoryPath = "path";
            Data.ItemVariantImages.IPhone63.FullPath = "fullpath";
            return new List<ItemVariantImage>()
            {
                Data.ItemVariantImages.Samsung81,
                Data.ItemVariantImages.IPhone61,
                Data.ItemVariantImages.IPhone62,
                Data.ItemVariantImages.IPhone63
            };
        }

        protected override Specification<ItemVariantImage> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemVariantImage>(i => i.Title.Contains("Samsung"));
        }
    }
}
