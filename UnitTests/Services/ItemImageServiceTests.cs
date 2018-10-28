using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Services
{
    public class ItemImageServiceTests : ServiceTestBase<ItemImage, IItemImageService>
    {
        public ItemImageServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        protected override async Task<IEnumerable<ItemImage>> GetCorrectNewEntitesAsync()
        {
            var firstItemDetails = await GetQueryable().FirstOrDefaultAsync();
            return await Task.FromResult(new List<ItemImage>()
            {
                new ItemImage("testImage1", @"image/jpg", 1, _service.GetStream(firstItemDetails))
            });
        }

        protected override async Task<IEnumerable<ItemImage>> GetIncorrectNewEntitesAsync()
        {
            var firstItemDetails = await GetQueryable().FirstOrDefaultAsync();
            return await Task.FromResult(new List<ItemImage>()
            {
                new ItemImage("", @"image/jpg", 1, _service.GetStream(firstItemDetails)),
                new ItemImage("test", @"image/jpg", 999, _service.GetStream(firstItemDetails)),
                new ItemImage("test", @"image/jpg", 1, null),
                new ItemImage("test",  @"image/exe", 1, _service.GetStream(firstItemDetails)),
            });
        }

        //[Fact]
        protected override async Task AssertCreateSuccessAsync(ItemImage itemImage)
        {
            var firstItemDetails = await GetQueryable().FirstOrDefaultAsync();
            await base.AssertCreateSuccessAsync(itemImage);
            Assert.True(_context.ItemImages.Contains(itemImage));
            var fileDest = File.Open(itemImage.FullPath, FileMode.Open);
            var fileInit = File.Open(firstItemDetails.FullPath, FileMode.Open);
            Assert.Equal(fileInit.Length, fileDest.Length);
            fileDest.Close();
            fileInit.Close();
            Directory.Delete(itemImage.DirectoryPath, true);
        }
        [Fact]
        public async Task GetStream()
        {
            var details = await GetQueryable().FirstOrDefaultAsync();
            using (var stream = _service.GetStream(details))
            {
                byte[] readBytes = new byte[stream.Length];
                int bytesCount = await stream.ReadAsync(readBytes, 0, (int)stream.Length);
                Assert.Equal(stream.Length, bytesCount);
            }
        }
        protected override async Task BeforeDeleteAsync(ItemImage itemImage)
        {
            await base.BeforeDeleteAsync(itemImage);
            string fileCopy = itemImage.FullPath + "_copy";
            if (!File.Exists(fileCopy))
            {
                File.Copy(itemImage.FullPath, fileCopy);
            };
            itemImage.FullPath = fileCopy;
            _context.ItemImages.Update(itemImage);
            await _context.SaveChangesAsync();
        }
        protected override async Task AfterDeleteAsync(ItemImage itemImage)
        {
            if (File.Exists(itemImage.FullPath))
                File.Delete(itemImage.FullPath);
            await Task.CompletedTask;
        }
        protected override async Task AssertRelatedDeleted(ItemImage itemImage)
        {
            await base.AssertRelatedDeleted(itemImage);
            Assert.False(File.Exists(itemImage.FullPath));
        }
        protected override async Task<IEnumerable<ItemImage>> GetCorrectEntitesForUpdateAsync()
        {
            var firstItemDetails = await GetQueryable().FirstOrDefaultAsync();
            firstItemDetails.Title = "Updated 1";
            return new List<ItemImage>()
            {
                firstItemDetails
            };
        }
        protected override async Task<IEnumerable<ItemImage>> GetIncorrectEntitesForUpdateAsync()
        {
            var firstItemDetails = await GetQueryable().FirstOrDefaultAsync();
            firstItemDetails.Title = "";
            return new List<ItemImage>()
            {
                firstItemDetails
            };
        }
        protected override Specification<ItemImage> GetEntitiesToDeleteSpecification()
        {
            return new Specification<ItemImage>(i => i.Title.Contains("Samsung"));
        }
    }
}
