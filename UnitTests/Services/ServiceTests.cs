using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    /// <summary>
    /// Base tests for any Service-derived instances
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TService"></typeparam>
    public abstract class ServiceTests<TEntity, TService>
       : TestBase<TEntity>
        where TService: IService<TEntity>
        where TEntity: Entity, ShallowClonable<TEntity>
    {
        protected TService _service { get; }

        public ServiceTests(ITestOutputHelper output)
           : base(output)
        {
            _service = Resolve<TService>();
        }

        [Fact]
        public virtual async Task CreateAsync()
        {
            foreach(TEntity correctNewEntity in GetCorrectNewEntites())
            {
                var newEntity = await _service.CreateAsync(correctNewEntity);
                await AssertCreateSuccessAsync(newEntity);
            }
        }

        protected abstract IEnumerable<TEntity> GetCorrectNewEntites();

        protected virtual Task AssertCreateSuccessAsync(TEntity created)
        {
            Assert.NotEqual(0, created.Id);
            Assert.NotNull(created.OwnerId);
            return Task.CompletedTask;
        }

        [Fact]
        public virtual async Task CreateAsyncThrowsValidationExceptionAsync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
            foreach (var incorrectEntity in GetIncorrectNewEntites())
                await Assert.ThrowsAsync<EntityValidationException>(() => _service.CreateAsync(incorrectEntity));
        }
        protected abstract IEnumerable<TEntity> GetIncorrectNewEntites();

        [Fact]
        public virtual async Task ReadSingleAsync()
        {
            var expected = await _context.Set<TEntity>().FirstOrDefaultAsync();
            var actual = await _service.ReadSingleAsync(new EntitySpecification<TEntity>(expected.Id));
            Equal(expected, actual);
        }

        [Fact]
        public virtual async Task ReadSingleAsyncThrowsNotFoundExceptionAsync()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.ReadSingleAsync(new EntitySpecification<TEntity>(9999)));
        }
        
        [Fact]
        public virtual async Task EnumerateAsync()
        {
            var expected = await _context.Set<TEntity>().ToListAsync();
            var actual = await _service.EnumerateAsync(new Specification<TEntity>(e => true));
            Equal(expected, actual);
        }

        [Fact]
        public virtual async Task EnumerateWithPaging()
        {
            int pageSize = 2;
            int totalCount = await _context.Set<TEntity>().CountAsync();
            //penultimate page
            int page = GetPageCount(totalCount, pageSize) - 1;
            page = page <= 0 ? 1 : page;
            var spec = new Specification<TEntity>(e => true);
            spec.SetPaging(page, pageSize);
            var actual = await _service.EnumerateAsync(spec);
            var expected = await _context.Set<TEntity>().Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
            Equal(actual, expected);
        }

        [Fact]
        public virtual async Task EnumerateOrderedAsync()
        {
            var fields = GetFieldsForOrdered();
            var expected = (await _context.Set<TEntity>().ToListAsync()).OrderBy(fields.First().Compile());
            foreach (var field in fields.Skip(1))
                expected = expected.ThenBy(field.Compile());
            var spec = new Specification<TEntity>(e => true);
            spec.OrderByExpressions.AddRange(fields);
            var actual = await _service.EnumerateAsync(spec);
            Equal(expected, actual);
        }

        [Fact]
        public virtual async Task EnumerateOrdereDescAsync()
        {
            var fields = GetFieldsForOrderedByDesc();
            var expected = (await _context.Set<TEntity>().ToListAsync()).OrderByDescending(fields.First().Compile());
            foreach (var field in fields.Skip(1))
                expected = expected.ThenByDescending(field.Compile());
            var spec = new Specification<TEntity>(e => true);
            spec.OrderByExpressions.AddRange(fields);
            spec.OrderByDesc = true;
            var actual = await _service.EnumerateAsync(spec);
            Equal(expected, actual);
        }

        [Fact]
        public virtual async Task EnumerateAsyncEmpty()
        {
            var expected = new List<TEntity>();
            var actual = await _service.EnumerateAsync(new Specification<TEntity>(e => false));
            Equal(expected, actual);
        }

        [Fact]
        public virtual async Task UpdateAsync()
        {
            foreach (var entityToUpdate in GetCorrectEntitesForUpdate())
                Equal(entityToUpdate, await _service.UpdateAsync(entityToUpdate));
        }

        protected abstract IEnumerable<TEntity> GetCorrectEntitesForUpdate();

        [Fact]
        public virtual async Task UpdateThrowsEntityValidationExceptionAsync()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAsync(null));
            var toUpdate = GetCorrectEntitesForUpdate();
            if (toUpdate.Any())
            {
                foreach (var incorrectEntity in GetIncorrectEntitesForUpdate())
                    await Assert.ThrowsAsync<EntityValidationException>(() => _service.UpdateAsync(incorrectEntity));
            }
        }

        protected virtual IEnumerable<TEntity> GetIncorrectEntitesForUpdate() => new List<TEntity>();

        [Fact]
        public virtual async Task DeleteSingleAsync()
        {
            var lastEntity = await _context.Set<TEntity>().LastOrDefaultAsync();
            await DeleteSingleEntityAsync(lastEntity);
        }

        [Fact]
        public virtual async Task DeleteAsync()
        {
            var entititesToDelete = await _context.EnumerateAsync(_logger, GetEntitiesToDeleteSpecification());
            foreach(var entityToDelete in entititesToDelete)
                await DeleteSingleEntityAsync(entityToDelete);
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
        public virtual async Task DeleteSingleAsyncThrowsNotFoundExceptionAsync()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteSingleAsync(new EntitySpecification<TEntity>(9999)));
        }

        [Fact]
        public virtual async Task CountTotalAsync()
        {
            int expected = await _context.Set<TEntity>().CountAsync();
            int actual = await _service.CountTotalAsync(new Specification<TEntity>(e => true));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public virtual async Task PageCountAsync()
        {
            int pageSize = 3;
            int totalCount = await _context.Set<TEntity>().CountAsync();
            int expected = GetPageCount(totalCount, pageSize);
            var spec = new Specification<TEntity>(e => true);
            spec.SetPaging(1, pageSize);
            int actual = await _service.PageCountAsync(spec);
            Assert.Equal(expected, actual);
        }

        protected virtual List<Expression<Func<TEntity, IComparable>>> GetFieldsForOrdered()
        {
            return new List<Expression<Func<TEntity, IComparable>>>()
            {
                (e => e.OwnerId.GetHashCode()),
                (e => e.Id)
            };
        }

        protected virtual List<Expression<Func<TEntity, IComparable>>> GetFieldsForOrderedByDesc()
        {
            return GetFieldsForOrdered();
        }

        protected int GetPageCount(int totalCount, int pageSize)
        {
            return (int)Math.Ceiling((decimal)totalCount / pageSize);
        }
    }
}
