using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using OctopusStore.Controllers;
using ApplicationCore.ViewModels;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class ItemImageControllerTests: ControllerTestBase<ItemImage, ItemImagesController, IItemImageService>
    {
        public ItemImageControllerTests(ITestOutputHelper output) :
            base(output)
        {
        }

        [Fact]
        public async Task Create()
        {
            var item = await _context.Items.Include(i => i.Images).FirstOrDefaultAsync();
            var lastImage = await GetQueryable().LastOrDefaultAsync();
            var imageMock = new Mock<IFormFile>();
            var filename = Path.GetFileName(lastImage.FullPath);
            var st = await service.GetStreamAsync(lastImage.Id);
            var ms = ReadFully(st);
            byte[] bytesExpected = ms.ToArray();
            st.Close();
            ms.Position = 0;
            imageMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            imageMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), CancellationToken.None))
                .Callback<Stream, CancellationToken>((stream, token) =>
                {
                    ms.CopyTo(stream);
                })
                .Returns(Task.CompletedTask);
            imageMock.Setup(_ => _.FileName).Returns(item.Id.ToString());
            imageMock.Setup(_ => _.Length).Returns(ms.Length);
            imageMock.Setup(_ => _.ContentType).Returns(lastImage.ContentType);
            imageMock.Setup(_ => _.Name).Returns(lastImage.Title);
            var image = imageMock.Object;
            var actual = await controller.PostForm(item.Id, image);

            var newImage = await GetQueryable().FirstOrDefaultAsync(i => i.Id == actual.Id);
            var streamActual = File.OpenRead(newImage.FullPath);
            byte[] bytesActual = new byte[streamActual.Length];
            streamActual.Read(bytesActual, 0, (int)streamActual.Length);
            streamActual.Close();
            Assert.Equal(bytesExpected.Length, bytesActual.Length);
        }
        [Fact]
        public async Task Get()
        {
            var imageExpected = await GetQueryable().FirstOrDefaultAsync();
            var streamExpected = File.OpenRead(imageExpected.FullPath);
            byte[] bytesExpected = new byte[streamExpected.Length];
            streamExpected.Read(bytesExpected, 0, (int)streamExpected.Length);
            streamExpected.Close();

            var imageActual = await controller.GetFile(imageExpected.Id) as FileStreamResult;
            var streamActual = imageActual.FileStream;
            byte[] bytesActual = new byte[streamActual.Length];
            streamActual.Read(bytesActual, 0, (int)streamActual.Length);
            streamActual.Close();

            Assert.Equal(bytesExpected, bytesActual);
        }
        [Fact]
        public async Task Put()
        {
            var imageExpected = await GetQueryable().FirstOrDefaultAsync();
            imageExpected.Title = "UPDATED";
            var expected = new ItemImageViewModel(imageExpected);
            var actual = await controller.Put(imageExpected.Id, new ItemImageViewModel(imageExpected));
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
            var imageActual = await GetQueryable().FirstOrDefaultAsync(i => i.Id == imageExpected.Id);
            Equal(imageExpected, imageActual);
            await Get();
        }
        [Fact]
        public async Task Index()
        {
            var imagesExpected = await GetQueryable().Where(i => i.RelatedId == 1).ToListAsync();
            var expected = new IndexViewModel<ItemImageViewModel>(1, 1, imagesExpected.Count, from i in imagesExpected select new ItemImageViewModel(i));
            var actual = await controller.Index(1);
            Equal(expected, actual);
        }
        [Fact]
        public async Task Delete()
        {
            var fileDetails = await GetQueryable().LastOrDefaultAsync();
            await CreateItemImageCopy(fileDetails);
            await controller.Delete(fileDetails.Id);

            Assert.False(File.Exists(fileDetails.FullPath));
        }
        public static MemoryStream ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            MemoryStream ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms;
        }
    }
}
