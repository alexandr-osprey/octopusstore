using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
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
        { }

        [Fact]
        public async Task AddAsync()
        {
            var firstItemDetails = await GetQueryable(context).FirstOrDefaultAsync();
            var addedDetails = new ItemImage("testuser", @"image/jpg", 1, service.GetStream(firstItemDetails));
            await service.AddAsync(addedDetails);
            Assert.True(context.ItemImages.Contains(addedDetails));
            var fileDest = File.Open(addedDetails.FullPath, FileMode.Open);
            var fileInit = File.Open(firstItemDetails.FullPath, FileMode.Open);
            Assert.Equal(fileInit.Length, fileDest.Length);
            fileDest.Close();
            Directory.Delete(addedDetails.DirectoryPath, true);
        }
        [Fact]
        public async Task GetSingleAsync()
        {
            var expected = await GetQueryable(context).FirstOrDefaultAsync();
            var actual = await service.GetSingleAsync(new Specification<ItemImage>(expected.Id));
            Assert.Equal(
                JsonConvert.SerializeObject(expected),
                JsonConvert.SerializeObject(actual));
        }
        [Fact]
        public async Task GetStream()
        {
            var details = await GetQueryable(context).FirstOrDefaultAsync();
            using (var stream = service.GetStream(details))
            {
                byte[] readBytes = new byte[stream.Length];
                int bytesCount = await stream.ReadAsync(readBytes, 0, (int)stream.Length);
                Assert.Equal(stream.Length, bytesCount);
            }
        }
        [Fact]
        public async Task DeleteAsync()
        {
            var fileDetails = await GetQueryable(context)
                .Where(i => i.Title.Contains("Samsung"))
                .FirstOrDefaultAsync();

            // make copy of file and pass test with it
            string fileCopy = fileDetails.FullPath + "_copy";
            File.Copy(fileDetails.FullPath, fileCopy);
            fileDetails.FullPath = fileCopy;
            context.ItemImages.Update(fileDetails);
            await context.SaveChangesAsync();

            var о = await service.GetSingleAsync(new Specification<ItemImage>(99));
            await service.DeleteAsync(new Specification<ItemImage>(fileDetails.Id));
            Assert.False(GetQueryable(context).Contains(fileDetails));
            Assert.False(File.Exists(fileDetails.FullPath));
        }
    }
}
