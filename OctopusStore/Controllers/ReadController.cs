using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace OctopusStore.Controllers
{
    public class ReadController<TService, TEntity, TViewModel, TDetailViewModel, TIndexViewModel> : Controller 
        where TService : IService<TEntity> where TEntity : Entity
        where TViewModel : ViewModel<TEntity>
        where TDetailViewModel : DetailViewModel<TEntity>
        where TIndexViewModel : IndexViewModel<TViewModel, TEntity>
    {
        protected readonly TService _serivce;
        protected readonly IAppLogger<IEntityController<TEntity>> _logger;
        protected string _entityName = typeof(TEntity).Name;
        protected int _maxTake = 200;
        protected int _defaultTake = 50;
    
        public ReadController(
            TService serivce, 
            IAppLogger<IEntityController<TEntity>> logger)
        {
            _serivce = serivce;
            _logger = logger;
            _logger.Name = typeof(TEntity).Name + "Controller";
        }

        protected async Task<TIndexViewModel> IndexAsync(ISpecification<TEntity> spec)
        {
            return await IndexAsync<TIndexViewModel, TViewModel>(spec);
        }
        protected async Task<TCustomIndexViewModel> IndexAsync<TCustomIndexViewModel, TCustomViewModel> (ISpecification<TEntity> spec) 
            where TCustomIndexViewModel: IndexViewModel<TCustomViewModel, TEntity> 
            where TCustomViewModel : ViewModel<TEntity>
        {
            if (spec.Take > _maxTake || spec.Skip < 0)
            {
                string message = $"Wrong page or page size parameter. Max page size: {_maxTake}";
                throw new BadRequestException(message);
            }
            try
            {
                var entities = await _serivce.ListAsync(spec);
                int totalPages = _serivce.PageCount(spec);
                int totalCount = _serivce.CountTotal(spec);
                return GetIndexViewModel<TCustomIndexViewModel, TCustomViewModel>(spec.Page, totalPages, totalCount, entities);
            }
            catch (Exception exception)
            {
                string message = $"Error getting entities of {_entityName}. spec: {spec}";
                _logger.Warn(exception, message);
                throw new InternalServerException(message);
            }
        }
        protected async Task<TIndexViewModel> IndexNotPagedAsync(ISpecification<TEntity> spec)
        {
            return await IndexByFunctionNotPagedAsync(spec, _serivce.ListAsync);
        }
        protected async Task<TIndexViewModel> IndexByFunctionNotPagedAsync(ISpecification<TEntity> spec, Func<ISpecification<TEntity>, Task<IEnumerable<TEntity>>> retrievingFunction)
        {
            try
            {
                spec.Take = _maxTake;
                var entities = await retrievingFunction(spec);
                return GetNotPagedIndexViewModel(entities);
            }
            catch (Exception exception)
            {
                string message = $"Error getting entities of {_entityName}. spec: {spec}";
                _logger.Warn(exception, message);
                throw new InternalServerException(message);
            }
        }
        protected async Task<TIndexViewModel> IndexByRelatedNotPagedAsync<TRelated>
            (Func<ISpecification<TRelated>, Task<IEnumerable<TEntity>>> retrievingFunction,
            ISpecification<TRelated> relatedSpec) 
            where TRelated : Entity
        {
            try
            {
                var entities = await retrievingFunction(relatedSpec);
                return GetNotPagedIndexViewModel(entities);
            }
            catch (Exception exception)
            {
                string message = $"Error getting related entities of {_entityName}. spec: {relatedSpec}";
                _logger.Warn(exception, message);
                throw new InternalServerException(message);
            }
        }
        protected async Task<TDetailViewModel> GetDetailAsync<TDetailSpecification>(TDetailSpecification spec) where TDetailSpecification: IDetailSpecification<TEntity>
        {
            var entityDetail = await GetEntityAsync(spec);
            return GetViewModel<TDetailViewModel>(entityDetail);
        }
        protected async Task<TViewModel> GetAsync(ISpecification<TEntity> spec)
        {
            var entity = await GetEntityAsync(spec);
            return GetViewModel<TViewModel>(entity);
        }
        protected async Task<TEntity> GetEntityAsync (ISpecification<TEntity> spec)
        {
            if (!_serivce.Exist(spec))
                throw new EntityNotFoundException($"{typeof(TEntity).Name} with spec {spec} does not exist. ");
            try
            {
                return await _serivce.GetSingleAsync(spec);
            }
            catch (Exception exception)
            {
                _logger.Warn(exception, $"Error getting {_entityName} with spec {spec}");
                throw new InternalServerException($"Error getting {_entityName}");
            }
        }
        protected TIndexViewModel GetNotPagedIndexViewModel(IEnumerable<TEntity> entities)
        {
            int totalCount = entities.Count();
            int page = totalCount == 0 ? 0 : 1;
            int totalPages = page;
            return GetIndexViewModel<TIndexViewModel, TViewModel>(page,  totalPages, totalCount, entities);
        }
        protected TCustomIndexViewModel GetIndexViewModel<TCustomIndexViewModel, TCustomViewModel>(int page, int totalPages, int totalCount, IEnumerable<TEntity> entities) 
            where TCustomIndexViewModel : IndexViewModel<TCustomViewModel, TEntity>
            where TCustomViewModel : ViewModel<TEntity>
        {
            return (TCustomIndexViewModel)Activator.CreateInstance(typeof(TCustomIndexViewModel), page, totalPages, totalCount, entities);
        }
        protected TCustomViewModel GetViewModel<TCustomViewModel>(TEntity entity) where TCustomViewModel : ViewModel<TEntity>
        {
            return (TCustomViewModel)Activator.CreateInstance(typeof(TCustomViewModel), entity);
        }
    }

    public class Response
    {
        public Response(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
