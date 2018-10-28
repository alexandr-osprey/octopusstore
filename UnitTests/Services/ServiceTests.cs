using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            foreach(var correctNewEntity in await GetCorrectNewEntitesAsync())
            {
                var newEntity = await _service.CreateAsync(correctNewEntity);
                await AssertCreateSuccessAsync(newEntity);
            }
        }
        protected abstract Task<IEnumerable<TEntity>> GetCorrectNewEntitesAsync();
        protected virtual async Task AssertCreateSuccessAsync(TEntity entity)
        {
            Assert.NotEqual(0, entity.Id);
            Assert.NotNull(entity.OwnerId);
            await Task.CompletedTask;
        }

        [Fact]
        public virtual async Task CreateAsyncThrowsValidationException()
        {
            foreach (var incorrectEntity in await GetIncorrectNewEntitesAsync())
            {
                await Assert.ThrowsAsync<EntityValidationException>(() => _service.CreateAsync(incorrectEntity));
            }
        }
        protected abstract Task<IEnumerable<TEntity>> GetIncorrectNewEntitesAsync();

        [Fact]
        public async Task ReadSingleAsync()
        {
            var expected = await _context.Set<TEntity>().FirstOrDefaultAsync();
            var actual = await _service.ReadSingleAsync(new EntitySpecification<TEntity>(expected.Id));
            Equal(expected, actual);
        }

        [Fact]
        public async Task ReadSingleAsyncThrowsNotFoundException()
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
        public async Task EnumerateAsyncEmpty()
        {
            var expected = new List<TEntity>();
            var actual = await _service.EnumerateAsync(new Specification<TEntity>(e => false));
            Equal(expected, actual);
        }
        [Fact]
        public async Task UpdateAsync()
        {
            foreach (var entityToUpdate in await GetCorrectEntitesForUpdateAsync())
            {
                Equal(entityToUpdate, await _service.UpdateAsync(entityToUpdate));
            }
        }
        protected abstract Task<IEnumerable<TEntity>> GetCorrectEntitesForUpdateAsync();

        [Fact]
        public async Task UpdateThrowsEntityValidationException()
        {
            foreach (var incorrectEntity in await GetIncorrectEntitesForUpdateAsync())
            {
                await Assert.ThrowsAsync<EntityValidationException>(() => _service.UpdateAsync(incorrectEntity));
            }
        }
        protected abstract Task<IEnumerable<TEntity>> GetIncorrectEntitesForUpdateAsync();

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
        public async Task DeleteSingleAsyncThrowsNotFoundException()
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
            int expected = (int)Math.Ceiling((decimal)totalCount / pageSize);
            int actual = await _service.PageCountAsync(new Specification<TEntity>(e => true) { Take = pageSize });
            Assert.Equal(expected, actual);
        }
    }
}
