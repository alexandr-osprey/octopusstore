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
    public class ItemImageServiceTests : ServiceTests<ItemImage, IItemImageService>
    {
        public ItemImageServiceTests(ITestOutputHelper output)
           : base(output)
        {
            _imageToCreateFrom = Data.ItemImages.Samsung71;
        }

        protected ItemImage _imageToCreateFrom;

        protected override IEnumerable<ItemImage> GetCorrectNewEntites()
        {
            return new List<ItemImage>()
            {
                new ItemImage("testImage1", _imageToCreateFrom.ContentType, _imageToCreateFrom.RelatedId, Service.GetStream(_imageToCreateFrom))
            };
        }

        protected override IEnumerable<ItemImage> GetIncorrectNewEntites()
        {
            var itemImage = Data.ItemImages.IPhone63;
            return new List<ItemImage>()
            {
                new ItemImage("", @"image/jpg", itemImage.RelatedId, Service.GetStream(itemImage)),
                new ItemImage("test", @"image/jpg", 999, Service.GetStream(itemImage)),
                new ItemImage("test", @"image/jpg", itemImage.RelatedId, null),
                new ItemImage("test",  @"image/exe", itemImage.RelatedId, Service.GetStream(itemImage)),
            };
        }

        //[Fact]
        protected override async Task AssertCreateSuccessAsync(ItemImage itemImage)
        {
            await base.AssertCreateSuccessAsync(itemImage);
            Assert.True(Context.ItemImages.Contains(itemImage));
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
            using (var stream = Service.GetStream(Data.ItemImages.Jacket1))
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
        protected override async Task AssertRelatedDeleted(ItemImage itemImage)
        {
            await base.AssertRelatedDeleted(itemImage);
            Assert.False(File.Exists(itemImage.FullPath));
        }

        protected override IEnumerable<ItemImage> GetCorrectEntitesForUpdate()
        {
            Data.ItemImages.Samsung81.Title = "Updated 1";
            return new List<ItemImage>() { Data.ItemImages.Samsung81 };
        }

        protected override IEnumerable<ItemImage> GetIncorrectEntitesForUpdate()
        {
            Data.ItemImages.Samsung81.Title = "";
            Data.ItemImages.IPhone61.ContentType = "txt";
            Data.ItemImages.IPhone62.DirectoryPath = "path";
            Data.ItemImages.IPhone63.FullPath = "fullpath";
            return new List<ItemImage>()
            {
                Data.ItemImages.Samsung81,
                Data.ItemImages.IPhone61,
                Data.ItemImages.IPhone62,
                Data.ItemImages.IPhone63
            };
        }

        protected override Specification<ItemImage> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemImage>(i => i.Title.Contains("Samsung"));
        }
    }
}
