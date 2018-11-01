using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OctopusStore.Controllers
{
    public class CRUDController<TService, TEntity, TViewModel>: Controller, IController<TEntity, TViewModel>
        where TService: IService<TEntity>
        where TEntity: Entity
        where TViewModel: EntityViewModel<TEntity>
    {
        protected readonly TService _service;
        protected readonly IAppLogger<IController<TEntity, TViewModel>> _logger;
        protected readonly string _entityName = typeof(TEntity).Name;
        protected readonly int _maxTake = 200;
        protected readonly int _defaultTake = 50;
        protected readonly IScopedParameters _scopedParameters;

        public CRUDController(
            TService service,
            IScopedParameters scopedParameters,
            IAppLogger<IController<TEntity, TViewModel>> logger)
        {
            _service = service;
            _scopedParameters = scopedParameters;
            _logger = logger;
            _logger.Name = _entityName + "Controller";
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            _scopedParameters.ClaimsPrincipal = User;
        }

        public virtual async Task<TViewModel> CreateAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new BadRequestException("Empty body");
            viewModel.Id = 0;
            var entity = await _service.CreateAsync(viewModel.ToModel());
            return GetViewModel<TViewModel>(entity);
        }

        public virtual async Task<TViewModel> ReadAsync(int id)
        {
            return await ReadAsync(new EntitySpecification<TEntity>(id));
        }

        public virtual async Task<TViewModel> UpdateAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new BadRequestException("Empty body");
            var entityToUpdate = await _service.ReadSingleAsync(viewModel.ToModel());
            var entitiy = await _service.UpdateAsync(viewModel.UpdateModel(entityToUpdate));
            return GetViewModel<TViewModel>(entitiy);
        }

        public virtual async Task<Response> DeleteAsync(int id)
        {
            return await DeleteAsync(new EntitySpecification<TEntity>(id));
        }

        public virtual async Task<Response> CheckUpdateAuthorizationAsync(int id)
        {
            var entity = await _service.ReadSingleAsync(new EntitySpecification<TEntity>(id));
            await _service.IdentityService.AuthorizeAsync(User, entity, OperationAuthorizationRequirements.Update, true);
            return new Response("You are authorized to update");
        }

        protected async Task<IndexViewModel<TCustomViewModel>> IndexAsync<TCustomViewModel>(Specification<TEntity> spec)
            //where TCustomIndexViewModel: EntityIndexViewModel<TCustomViewModel, TEntity>
            where TCustomViewModel : EntityViewModel<TEntity>
        {
            if (spec.Take > _maxTake || spec.Skip < 0)
            {
                string message = $"Wrong page or page size parameter. Max page size: {_maxTake}";
                throw new BadRequestException(message);
            }
            var entities = await _service.EnumerateAsync(spec);
            int totalCount = await _service.CountTotalAsync(spec);
            int totalPages = await _service.PageCountAsync(spec);
            return GetIndexViewModel<TCustomViewModel>(spec.Page, totalPages, totalCount, entities);
        }

        protected virtual async Task<IndexViewModel<TViewModel>> IndexAsync(Specification<TEntity> spec)
        {
            return await IndexAsync<TViewModel>(spec);
        }

        protected async Task<IndexViewModel<TViewModel>> IndexByFunctionNotPagedAsync<TCustom>(Func<Specification<TCustom>, Task<IEnumerable<TEntity>>> retrievingFunction, Specification<TCustom> spec)
            where TCustom : class
        {
            return GetNotPagedIndexViewModel(await retrievingFunction(spec));
        }

        protected async Task<IndexViewModel<TViewModel>> IndexByRelatedNotPagedAsync<TRelated>
            (Func<Specification<TRelated>, Task<IEnumerable<TEntity>>> retrievingFunction,
            Specification<TRelated> relatedSpec)
            where TRelated : Entity
        {
            return GetNotPagedIndexViewModel(await retrievingFunction(relatedSpec));
        }

        protected async Task<IndexViewModel<TViewModel>> IndexNotPagedAsync(Specification<TEntity> spec)
        {
            spec.SetPaging(1, _maxTake);
            return GetNotPagedIndexViewModel(await _service.EnumerateAsync(spec));
        }

        protected async Task<TDetailViewModel> GetDetailAsync<TDetailViewModel>(Specification<TEntity> spec) where TDetailViewModel: EntityViewModel<TEntity>
        {
            return GetViewModel<TDetailViewModel>(await _service.ReadSingleAsync(spec));
        }

        protected async Task<TViewModel> ReadAsync(Specification<TEntity> spec)
        {
            return GetViewModel<TViewModel>(await _service.ReadSingleAsync(spec));
        }

        protected IndexViewModel<TViewModel> GetNotPagedIndexViewModel(IEnumerable<TEntity> entities)
        {
            int totalCount = entities.Count();
            int page = totalCount == 0 ? 0 : 1;
            int totalPages = page;
            return GetIndexViewModel<TViewModel>(page, totalPages, totalCount, entities);
        }

        protected IndexViewModel<TCustomViewModel> GetIndexViewModel<TCustomViewModel>(int page, int totalPages, int totalCount, IEnumerable<TEntity> entities)
            where TCustomViewModel: EntityViewModel<TEntity>
        {
            var viewModels = from e in entities select GetViewModel<TCustomViewModel>(e);
            return IndexViewModel<TCustomViewModel>.FromEnumerable(page, totalPages, totalCount, viewModels.OrderBy(v => v.Id));
            //var types = new Type[] { typeof(int), typeof(int), typeof(int), typeof(IEnumerable<TEntity>) };
            //return (TCustomIndexViewModel)ast.GetActivator(typeof(TCustomIndexViewModel), types)(page, totalPages, totalCount, entities);
        }

        protected TCustomViewModel GetViewModel<TCustomViewModel>(TEntity entity) where TCustomViewModel: EntityViewModel<TEntity>
        {
            return (TCustomViewModel)new ActivatorsStorage().GetActivator(typeof(TCustomViewModel), typeof(TEntity))(entity);
        }

        protected virtual async Task<Response> DeleteSingleAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            await _service.DeleteSingleAsync(spec);
            return new Response($"Deleted {_entityName}(s) according to spec {spec}");
        }

        protected virtual async Task<Response> DeleteAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            int deleted = await _service.DeleteAsync(spec);
            return new Response($"Deleted {deleted} {_entityName}(s)");
        }
    }
}
