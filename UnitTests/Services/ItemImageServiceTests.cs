//using ApplicationCore.Entities;
//using ApplicationCore.Exceptions;
//using ApplicationCore.Interfaces;
//using ApplicationCore.Specifications;
//using Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;
//using Xunit.Abstractions;

//namespace UnitTests.Services
//{
//    public class ItemImageServiceTests: ServiceTestBase<ItemImage, IItemImageService>
//    {
//        public ItemImageServiceTests(ITestOutputHelper output)
//           : base(output)
//        {
//        }

//        [Fact]
//        public async Task CreateAsync()
//        {
//            var firstItemDetails = await GetQueryable().FirstOrDefaultAsync();
//            var addedDetails = new ItemImage("testImage1", "testuser", @"image/jpg", 1, _service.GetStream(firstItemDetails));
//            await _service.CreateAsync(addedDetails);
//            Assert.True(context.ItemImages.Contains(addedDetails));
//            var fileDest = File.Open(addedDetails.FullPath, FileMode.Open);
//            var fileInit = File.Open(firstItemDetails.FullPath, FileMode.Open);
//            Assert.Equal(fileInit.Length, fileDest.Length);
//            fileDest.Close();
//            Directory.Delete(addedDetails.DirectoryPath, true);
//        }
//        [Fact]
//        public async Task ReadSingleAsync()
//        {
//            var expected = await GetQueryable().FirstOrDefaultAsync();
//            var actual = await _service.ReadSingleAsync(new EntitySpecification<ItemImage>(expected.Id));
//            Assert.Equal(
//                JsonConvert.SerializeObject(expected),
//                JsonConvert.SerializeObject(actual));
//        }
//        [Fact]
//        public async Task GetStream()
//        {
//            var details = await GetQueryable().FirstOrDefaultAsync();
//            using (var stream = _service.GetStream(details))
//            {
//                byte[] readBytes = new byte[stream.Length];
//                int bytesCount = await stream.ReadAsync(readBytes, 0, (int)stream.Length);
//                Assert.Equal(stream.Length, bytesCount);
//            }
//        }
//        [Fact]
//        public async Task DeleteAsync()
//        {
//            var fileDetails = await GetQueryable()
//           .Where(i => i.Title.Contains("Samsung"))
//           .FirstOrDefaultAsync();

//            // make copy of file and pass test with it
//            string fileCopy = fileDetails.FullPath + "_copy";
//            File.Copy(fileDetails.FullPath, fileCopy);
//            fileDetails.FullPath = fileCopy;
//            try
//            {
//                context.ItemImages.Update(fileDetails);
//                await context.SaveChangesAsync();

//                await Assert.ThrowsAsync<EntityNotFoundException>(async () => await _service.ReadSingleAsync(new EntitySpecification<ItemImage>(99)));
//                await _service.DeleteAsync(new EntitySpecification<ItemImage>(fileDetails.Id));
//                Assert.False(GetQueryable().Contains(fileDetails));
//                Assert.False(File.Exists(fileDetails.FullPath));
//            }
//            finally
//            {
//                if (File.Exists(fileCopy))
//                    File.Delete(fileCopy);
//            }
//        }
//    }
//}
