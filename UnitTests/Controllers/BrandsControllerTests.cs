using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public class BrandsControllerTests: ControllerTests<Brand, BrandViewModel, IBrandsController, IBrandService>
    {
        public BrandsControllerTests(ITestOutputHelper output): base(output)
        {
        }
        
        [Fact]
        public async Task IndexAsync()
        {
            var entities = await _context.Set<Brand>().ToListAsync();
            var expected = IndexViewModel<BrandViewModel>.FromEnumerableNotPaged(from e in entities select ToViewModel(e));
            var actual = await _controller.IndexAsync();
            Equal(expected, actual);
        }

        protected override async Task<IEnumerable<Brand>> GetCorrectEntitiesToCreateAsync()
        {
            var entities = await _context.Set<Brand>().Take(3).ToListAsync();
            entities.ForEach(e => e.Id = 0);
            return entities;
        }

        protected override async Task<IEnumerable<Brand>> GetCorrectEntitiesToUpdateAsync()
        {
            return await _context.Set<Brand>().Skip(1).Take(3).ToListAsync();
        }

        protected override BrandViewModel ToViewModel(Brand entity)
        {
            return new BrandViewModel()
            {
                Id = entity.Id,
                Title = entity.Title
            };
        }
    }
}
