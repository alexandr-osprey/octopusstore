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
        protected readonly TService _service;

        public ServiceTests(ITestOutputHelper output)
           : base(output)
        {
            _service = Resolve<TService>();
        }

        [Fact]
        public virtual async Task CreateAsync()
        {
            foreach(var correctNewEntity in GetCorrectNewEntites())
            {
                var newEntity = await _service.CreateAsync(correctNewEntity);
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
                await Assert.ThrowsAsync<EntityValidationException>(() => _service.CreateAsync(incorrectEntity));
            }
        }
        protected abstract IEnumerable<TEntity> GetIncorrectNewEntites();

        [Fact]
        public async Task ReadSingleAsync()
        {
            var expected = await _context.Set<TEntity>().FirstOrDefaultAsync();
            var actual = await _service.ReadSingleAsync(new EntitySpecification<TEntity>(expected.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task ReadSingleAsyncThrowsNotFoundExceptionAsync()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.ReadSingleAsync(new EntitySpecification<TEntity>(9999)));
        }
        
        [Fact]
        public async Task EnumerateAsync()
        {
            var expected = await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            var actual = await _service.EnumerateAsync(new Specification<TEntity>(e => true));
            Equal(expected, actual);
        }

        [Fact]
        public async Task EnumerateWithPaging()
        {
            int pageSize = 2;
            int totalCount = await _context.Set<TEntity>().CountAsync();
            //penultimate page
            int page = GetPageCount(totalCount, pageSize) - 1;
            page = page <= 0 ? 1 : page;
            var spec = new Specification<TEntity>(e => true);
            spec.SetPaging(page, pageSize);
            var actual = await _service.EnumerateAsync(spec);
            var expected = await _context.Set<TEntity>().AsNoTracking().Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
            Equal(actual, expected);
        }
        [Fact]
        public async Task EnumerateAsyncEmpty()
        {
            var expected = new List<TEntity>();
            var actual = await _service.EnumerateAsync(new Specification<TEntity>(e => false));
            Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            foreach (var entityToUpdate in GetCorrectEntitesForUpdate())
            {
                Equal(entityToUpdate, await _service.UpdateAsync(entityToUpdate));
            }
        }

        protected abstract IEnumerable<TEntity> GetCorrectEntitesForUpdate();

        [Fact]
        public async Task UpdateThrowsEntityValidationExceptionAsync()
        {
            foreach (var incorrectEntity in GetIncorrectEntitesForUpdate())
            {
                await Assert.ThrowsAsync<EntityValidationException>(() => _service.UpdateAsync(incorrectEntity));
            }
        }
        protected abstract IEnumerable<TEntity> GetIncorrectEntitesForUpdate();

        [Fact]
        public async Task DeleteSingleAsync()
        {
            var lastEntity = await _context.Set<TEntity>().LastOrDefaultAsync();
            await DeleteSingleEntityAsync(lastEntity);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var entititesToDelete = await _context.EnumerateAsync(_logger, GetEntitiesToDeleteSpecification());
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
                await _service.DeleteSingleAsync(new EntitySpecification<TEntity>(entity.Id));
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
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteSingleAsync(new EntitySpecification<TEntity>(9999)));
        }


        [Fact]
        public async Task CountTotalAsync()
        {
            int expected = await _context.Set<TEntity>().CountAsync();
            int actual = await _service.CountTotalAsync(new Specification<TEntity>(e => true));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task PageCountAsync()
        {
            int pageSize = 3;
            int totalCount = await _context.Set<TEntity>().CountAsync();
            int expected = GetPageCount(totalCount, pageSize);
            var spec = new Specification<TEntity>(e => true);
            spec.SetPaging(1, pageSize);
            int actual = await _service.PageCountAsync(spec);
            Assert.Equal(expected, actual);
        }

        protected int GetPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }
    }
}
