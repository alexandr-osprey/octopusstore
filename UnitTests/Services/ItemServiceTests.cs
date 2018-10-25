using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Infrastructure.Data;

namespace UnitTests.Services
{
    public class ItemServiceTests: ServiceTestBase<Item, IItemService>
    {
        public ItemServiceTests(ITestOutputHelper output)
           : base(output)
        {
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var item = await GetQueryable(context).LastOrDefaultAsync();
            var imagePaths = new List<string>();
            foreach (var image in item.Images)
            {
                imagePaths.Add(image.FullPath);
                File.Copy(image.FullPath, image.FullPath + ".backup");
            }
            try
            {
                await service.DeleteAsync(new ItemDetailSpecification(item.Id));
                Assert.False(GetQueryable(context).Contains(item));
                foreach (var image in item.Images)
                    Assert.False(context.ItemImages.Contains(image));
                foreach (var variant in item.ItemVariants)
                    Assert.False(context.ItemVariants.Contains(variant));
            }
            finally
            {
                foreach (var path in imagePaths)
                {
                    Assert.False(File.Exists(path));
                    File.Move(path + ".backup", path);
                }
            }
        }
        protected override IQueryable<Item> GetQueryable(DbContext context)
        {
            return base.GetQueryable(context).Include(i => i.Images)
                    .Include(i => i.ItemVariants)
                        .ThenInclude(i => i.ItemVariantCharacteristicValues);
        }
    }
}
