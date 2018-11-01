using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests.Controllers
{
    public abstract class ControllerTests<TEntity, TViewModel, TController, TService>
       : TestBase<TEntity>
        where TEntity : Entity
        where TViewModel: EntityViewModel<TEntity>
        where TController : IController<TEntity, TViewModel>
        where TService : IService<TEntity>
    {
        protected readonly TController _controller;
        protected readonly TService _service;

        public ControllerTests(ITestOutputHelper output): base(output)
        {
            _controller = Resolve<TController>();
            _service = Resolve<TService>();
            (_controller as Controller).ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task CreateAsync()
        {
            foreach(var entity in await GetCorrectEntitiesToCreateAsync())
            {
                var expected = ToViewModel(entity);
                var actual = await _controller.CreateAsync(expected);
                //Assert.True(await _context.Set<TEntity>().AnyAsync(e => e.Id == entity.Id));
                await AssertCreateSuccess(expected, actual);
            }
        }

        protected abstract Task<IEnumerable<TEntity>> GetCorrectEntitiesToCreateAsync();
        protected virtual async Task AssertCreateSuccess(TViewModel expected, TViewModel actual)
        {
            Assert.NotEqual(0, actual.Id);
            expected.Id = actual.Id;
            Equal(expected, actual);
            await Task.CompletedTask;
        }
        protected abstract TViewModel ToViewModel(TEntity entity);

        [Fact]
        public async Task ReadAsync()
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync();
            var expected = ToViewModel(entity);
            var actual = await _controller.ReadAsync(entity.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            foreach(var entity in await GetCorrectEntitiesToUpdateAsync())
            {
                var beforeUpdate = await _context.Set<TEntity>().AsNoTracking().FirstAsync(e => e.Id == entity.Id);
                var expected = ToViewModel(entity);
                var actual = await _controller.UpdateAsync(expected);
                await AssertUpdateSuccessAsync(beforeUpdate, expected, actual);
            }
        }
        protected abstract Task<IEnumerable<TEntity>> GetCorrectEntitiesToUpdateAsync();
        protected virtual async Task AssertUpdateSuccessAsync(TEntity beforeUpdate, TViewModel expected, TViewModel actual)
        {
            Equal(expected, actual);
            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var entity = await _context.Set<TEntity>().LastOrDefaultAsync();
            await _controller.DeleteAsync(entity.Id);
            Assert.False(await _context.Set<TEntity>().ContainsAsync(entity));
        }
        public int GetPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }
    }
}
