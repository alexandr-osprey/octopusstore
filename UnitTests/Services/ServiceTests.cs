using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public abstract class ServiceTests<TEntity, TService>
       : TestBase<TEntity>
        where TService: IService<TEntity>
        where TEntity: Entity
    {
        protected TService Service { get; }

        public ServiceTests(ITestOutputHelper output)
           : base(output)
        {
            Service = Resolve<TService>();
        }

        [Fact]
        public virtual async Task CreateAsync()
        {
            foreach(TEntity correctNewEntity in GetCorrectNewEntites())
            {
                var newEntity = await Service.CreateAsync(correctNewEntity);
                await AssertCreateSuccessAsync(newEntity);
            }
        }

        protected abstract IEnumerable<TEntity> GetCorrectNewEntites();

        protected virtual async Task AssertCreateSuccessAsync(TEntity created)
        {
            Assert.NotEqual(0, created.Id);
            Assert.NotNull(created.OwnerId);
            await Task.CompletedTask;
        }

        [Fact]
        public virtual async Task CreateAsyncThrowsValidationExceptionAsync()
        {
            foreach (var incorrectEntity in GetIncorrectNewEntites())
            {
                await Assert.ThrowsAsync<EntityValidationException>(() => Service.CreateAsync(incorrectEntity));
            }
        }
        protected abstract IEnumerable<TEntity> GetIncorrectNewEntites();

        [Fact]
        public async Task ReadSingleAsync()
        {
            var expected = await Context.Set<TEntity>().FirstOrDefaultAsync();
            var actual = await Service.ReadSingleAsync(new EntitySpecification<TEntity>(expected.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task ReadSingleAsyncThrowsNotFoundExceptionAsync()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => Service.ReadSingleAsync(new EntitySpecification<TEntity>(9999)));
        }
        
        [Fact]
        public async Task EnumerateAsync()
        {
            var expected = await Context.Set<TEntity>().ToListAsync();
            var actual = await Service.EnumerateAsync(new Specification<TEntity>(e => true));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateWithPaging()
        {
            int pageSize = 2;
            int totalCount = await Context.Set<TEntity>().CountAsync();
            //penultimate page
            int page = GetPageCount(totalCount, pageSize) - 1;
            page = page <= 0 ? 1 : page;
            var spec = new Specification<TEntity>(e => true);
            spec.SetPaging(page, pageSize);
            var actual = await Service.EnumerateAsync(spec);
            var expected = await Context.Set<TEntity>().Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
            Equal(actual, expected);
        }

        [Fact]
        public async Task EnumerateAsyncEmpty()
        {
            var expected = new List<TEntity>();
            var actual = await Service.EnumerateAsync(new Specification<TEntity>(e => false));
            Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            foreach (var entityToUpdate in GetCorrectEntitesForUpdate())
            {
                Equal(entityToUpdate, await Service.UpdateAsync(entityToUpdate));
            }
        }

        protected abstract IEnumerable<TEntity> GetCorrectEntitesForUpdate();

        [Fact]
        public async Task UpdateThrowsEntityValidationExceptionAsync()
        {
            foreach (var incorrectEntity in GetIncorrectEntitesForUpdate())
            {
                await Assert.ThrowsAsync<EntityValidationException>(() => Service.UpdateAsync(incorrectEntity));
            }
        }
        protected abstract IEnumerable<TEntity> GetIncorrectEntitesForUpdate();

        [Fact]
        public async Task DeleteSingleAsync()
        {
            var lastEntity = await Context.Set<TEntity>().LastOrDefaultAsync();
            await DeleteSingleEntityAsync(lastEntity);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var entititesToDelete = await Context.EnumerateAsync(Logger, GetEntitiesToDeleteSpecification());
            foreach(var entityToDelete in entititesToDelete)
            {
                await DeleteSingleEntityAsync(entityToDelete);
            }
        }

        protected async Task DeleteSingleEntityAsync(TEntity entity)
        {
            try
            {
                await BeforeDeleteAsync(entity);
                await Service.DeleteSingleAsync(new EntitySpecification<TEntity>(entity.Id));
                Assert.False(await GetQueryable().AnyAsync(e => e == entity));
                await AssertRelatedDeleted(entity);
            }
            finally
            {
                await AfterDeleteAsync(entity);
            }
        }

        protected virtual async Task BeforeDeleteAsync(TEntity entity)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task AfterDeleteAsync(TEntity entity)
        {
            await Task.CompletedTask;
        }

        protected abstract Specification<TEntity> GetEntitiesToDeleteSpecification();

        protected virtual async Task AssertRelatedDeleted(TEntity entity)
        {
            await Task.CompletedTask;
        }

        [Fact]
        public async Task DeleteSingleAsyncThrowsNotFoundExceptionAsync()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => Service.DeleteSingleAsync(new EntitySpecification<TEntity>(9999)));
        }


        [Fact]
        public async Task CountTotalAsync()
        {
            int expected = await Context.Set<TEntity>().CountAsync();
            int actual = await Service.CountTotalAsync(new Specification<TEntity>(e => true));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task PageCountAsync()
        {
            int pageSize = 3;
            int totalCount = await Context.Set<TEntity>().CountAsync();
            int expected = GetPageCount(totalCount, pageSize);
            var spec = new Specification<TEntity>(e => true);
            spec.SetPaging(1, pageSize);
            int actual = await Service.PageCountAsync(spec);
            Assert.Equal(expected, actual);
        }

        protected int GetPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }
    }
}
