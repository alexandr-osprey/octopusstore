using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        protected TController Controller { get; }
        protected TService Service { get; }

        public ControllerTests(ITestOutputHelper output): base(output)
        {
            Controller = Resolve<TController>();
            Service = Resolve<TService>();
            (Controller as Controller).ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task CreateAsync()
        {
            foreach(var entity in GetCorrectEntitiesToCreate())
            {
                var expected = ToViewModel(entity);
                var actual = await Controller.CreateAsync(expected);
                //Assert.True(await _context.Set<TEntity>().AnyAsync(e => e.Id == entity.Id));
                await AssertCreateSuccessAsync(expected, actual);
            }
        }

        protected abstract IEnumerable<TEntity> GetCorrectEntitiesToCreate();
        protected virtual Task AssertCreateSuccessAsync(TViewModel expected, TViewModel actual)
        {
            Assert.NotEqual(0, actual.Id);
            expected.Id = actual.Id;
            Equal(expected, actual);
            return Task.CompletedTask;
        }
        protected abstract TViewModel ToViewModel(TEntity entity);

        [Fact]
        public async Task ReadAsync()
        {
            var entity = await Context.Set<TEntity>().FirstOrDefaultAsync();
            var expected = ToViewModel(entity);
            var actual = await Controller.ReadAsync(entity.Id);
            Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            foreach(var viewModel in GetCorrectViewModelsToUpdate())
            {
                var beforeUpdate = await Context.Set<TEntity>().FirstAsync(e => e.Id == viewModel.Id);
                var actual = await Controller.UpdateAsync(viewModel);
                await AssertUpdateSuccessAsync(beforeUpdate, viewModel, actual);
            }
        }

        protected abstract IEnumerable<TViewModel> GetCorrectViewModelsToUpdate();

        protected virtual Task AssertUpdateSuccessAsync(TEntity beforeUpdate, TViewModel expected, TViewModel actual)
        {
            Equal(expected, actual);
            return Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var entity = await Context.Set<TEntity>().LastAsync();
            await Controller.DeleteAsync(entity.Id);
            Assert.False(await Context.Set<TEntity>().ContainsAsync(entity));
        }

        public int GetPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }

        protected override void Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }

        protected override void Equal<T>(T expected, T actual)
        {
            Assert.Equal(
                JsonConvert.SerializeObject(expected, Formatting.None, jsonSettings),
                JsonConvert.SerializeObject(actual, Formatting.None, jsonSettings));
        }
    }
}
