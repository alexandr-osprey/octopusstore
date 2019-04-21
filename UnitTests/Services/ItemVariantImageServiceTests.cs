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
    public class ItemVariantImageServiceTests : ServiceTests<ItemVariantImage, IItemVariantImageService>
    {
        public ItemVariantImageServiceTests(ITestOutputHelper output)
           : base(output)
        {
            _imageToCreateFrom = _data.ItemVariantImages.Entities.FirstOrDefault();
        }

        protected ItemVariantImage _imageToCreateFrom;

        protected override IEnumerable<ItemVariantImage> GetCorrectNewEntites()
        {
            return new List<ItemVariantImage>()
            {
                new ItemVariantImage("testImage1", _imageToCreateFrom.ContentType, _imageToCreateFrom.RelatedId, _service.GetStream(_imageToCreateFrom))
            };
        }

        protected override IEnumerable<ItemVariantImage> GetIncorrectNewEntites()
        {
            var itemImage = _data.ItemVariantImages.Entities.LastOrDefault();
            return new List<ItemVariantImage>()
            {
                new ItemVariantImage("", @"image/jpg", itemImage.RelatedId, _service.GetStream(itemImage)),
                new ItemVariantImage("test", @"image/jpg", 999, _service.GetStream(itemImage)),
                new ItemVariantImage("test", @"image/jpg", itemImage.RelatedId, null),
                new ItemVariantImage("test",  @"image/exe", itemImage.RelatedId, _service.GetStream(itemImage)),
            };
        }

        //[Fact]
        protected override async Task AssertCreateSuccessAsync(ItemVariantImage itemImage)
        {
            await base.AssertCreateSuccessAsync(itemImage);
            Assert.True(_context.ItemVariantImages.Contains(itemImage));
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
            using (var stream = _service.GetStream(_data.ItemVariantImages.Entities.ElementAt(3)))
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
            var first = _data.ItemVariantImages.Entities.FirstOrDefault();
            first.Title = "Updated 1";
            return new List<ItemVariantImage>() { first };
        }

        protected override IEnumerable<ItemVariantImage> GetIncorrectEntitesForUpdate()
        {
            var first = _data.ItemVariantImages.Entities.ElementAt(0);
            var second = _data.ItemVariantImages.Entities.ElementAt(0);
            var third = _data.ItemVariantImages.Entities.ElementAt(0);
            var fourth = _data.ItemVariantImages.Entities.ElementAt(0);
            first.Title = "";
            second.ContentType = "txt";
            third.DirectoryPath = "path";
            fourth.FullPath = "fullpath";
            return new List<ItemVariantImage>()
            {
                first,
                second,
                third,
                fourth
            };
        }

        protected override Specification<ItemVariantImage> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemVariantImage>(i => i.Title.Contains("Samsung"));
        }
    }
}
