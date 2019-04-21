using ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ApplicationCore.ViewModels;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    public class ItemVariantImageControllerTests: ControllerTests<ItemVariantImage, ItemVariantImageViewModel, IItemVariantImagesController, IItemVariantImageService>
    {
        public ItemVariantImageControllerTests(ITestOutputHelper output): base(output)
        {
        }

        [Fact]
        public async Task CreateImageAsync()
        {
            var itemVariant = _data.Items.SamsungS10;
            var imageToCopy = _data.ItemVariantImages.Entities.FirstOrDefault();
            var imageMock = new Mock<IFormFile>();
            var filename = Path.GetFileName(imageToCopy.FullPath);
            var st = await _service.GetStreamAsync(imageToCopy.Id);
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
            imageMock.Setup(_ => _.FileName).Returns(itemVariant.Id.ToString());
            imageMock.Setup(_ => _.Length).Returns(ms.Length);
            imageMock.Setup(_ => _.ContentType).Returns(imageToCopy.ContentType);
            imageMock.Setup(_ => _.Name).Returns(imageToCopy.Title);
            var image = imageMock.Object;
            var actual = await _controller.PostFormAsync(itemVariant.Id, image);

            var newImage = await GetQueryable().FirstOrDefaultAsync(i => i.Id == actual.Id);
            var streamActual = File.OpenRead(newImage.FullPath);
            byte[] bytesActual = new byte[streamActual.Length];
            streamActual.Read(bytesActual, 0, (int)streamActual.Length);
            streamActual.Close();
            Assert.Equal(bytesExpected.Length, bytesActual.Length);
        }

        [Fact]
        public async Task GetFileAsync()
        {
            var imageExpected = _data.ItemVariantImages.Entities.ElementAt(2);
            var streamExpected = File.OpenRead(imageExpected.FullPath);
            byte[] bytesExpected = new byte[streamExpected.Length];
            streamExpected.Read(bytesExpected, 0, (int)streamExpected.Length);
            streamExpected.Close();

            var imageActual = await _controller.GetFileAsync(imageExpected.Id) as FileStreamResult;
            var streamActual = imageActual.FileStream;
            byte[] bytesActual = new byte[streamActual.Length];
            streamActual.Read(bytesActual, 0, (int)streamActual.Length);
            streamActual.Close();

            Assert.Equal(bytesExpected, bytesActual);
        }

        [Fact]
        public async Task UpdateWithoutImageAsync()
        {
            var imageExpected = _data.ItemVariantImages.Entities.ElementAt(3);
            imageExpected.Title = "UPDATED";
            var expected = new ItemVariantImageViewModel(imageExpected);
            var actual = await _controller.UpdateAsync(new ItemVariantImageViewModel(imageExpected));
            Equal(expected, actual);
            var imageActual = await GetQueryable().FirstOrDefaultAsync(i => i.Id == imageExpected.Id);
            imageExpected.RelatedEntity = null;
            Equal(imageExpected, imageActual);
            await GetFileAsync();
        }

        [Fact]
        public async Task IndexAsync()
        {
            var itemVariant = _data.ItemVariants.SamsungGalaxyS10128GBBlack;
            var imagesExpected = await GetQueryable().Where(i => i.RelatedId == itemVariant.Id).ToListAsync();
            var expected = new IndexViewModel<ItemVariantImageViewModel>(1, 1, imagesExpected.Count, from i in imagesExpected select new ItemVariantImageViewModel(i));
            var actual = await _controller.IndexAsync(itemVariant.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteWithImageAsync()
        {
            var fileInfo = _data.ItemVariantImages.Entities.ElementAt(5);
            //await CreateItemImageCopy(fileInfo);
            await _controller.DeleteAsync(fileInfo.Id);

            Assert.False(File.Exists(fileInfo.FullPath));
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

        protected override Task AssertUpdateSuccessAsync(ItemVariantImage beforeUpdate, ItemVariantImageViewModel expected, ItemVariantImageViewModel actual)
        {
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(beforeUpdate.RelatedId, actual.RelatedId);
            Assert.Equal(beforeUpdate.ContentType, actual.ContentType);
            return Task.CompletedTask;
        }

        protected override IEnumerable<ItemVariantImage> GetCorrectEntitiesToCreate()
        {
            return new List<ItemVariantImage>();
        }

        protected override ItemVariantImageViewModel ToViewModel(ItemVariantImage entity)
        {
            return new ItemVariantImageViewModel()
            {
                Id = entity.Id,
                RelatedId = entity.RelatedId,
                ContentType = entity.ContentType,
                Title = entity.Title,
            };
        }

        protected override IEnumerable<ItemVariantImageViewModel> GetCorrectViewModelsToUpdate()
        {
            return new List<ItemVariantImageViewModel>()
            {
                new ItemVariantImageViewModel()
                {
                    Id = _data.ItemVariants.DanielePatriciClutch1.Images.FirstOrDefault().Id,
                    RelatedId = 999,
                    ContentType = "ADFF",
                    Title = "UPDATED"
                }
            };
        }
    }
}
