using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Extensions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Controllers;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OspreyStore.Controllers
{
    public class CRUDController<TService, TEntity, TViewModel> : Controller, IController<TEntity, TViewModel>
        where TService : IService<TEntity>
        where TEntity : Entity
        where TViewModel : EntityViewModel<TEntity>
    {
        protected TService _service { get; }
        protected IActivatorService _activatorService { get; }
        protected IIdentityService _identityService { get; }
        protected IAppLogger<IController<TEntity, TViewModel>> _logger { get; }
        protected string _entityName { get; } = typeof(TEntity).Name;
        protected int _maxTake { get; } = 200;
        protected int _defaultTake { get; } = 10;
        public IScopedParameters ScopedParameters { get; }

        public CRUDController(
            TService service,
            IActivatorService activatorService,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<TEntity, TViewModel>> logger)
        {
            _service = service;
            _activatorService = activatorService;
            ScopedParameters = scopedParameters;
            _logger = logger;
            _identityService = identityService;
            _logger.Name = _entityName + "Controller";
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ScopedParameters.ClaimsPrincipal = User;
        }

        public virtual async Task<TViewModel> CreateAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new BadRequestException("Empty body");
            viewModel.Id = 0;
            var entity = await _service.CreateAsync(viewModel.ToModel());
            return await GetViewModelAsync<TViewModel>(entity);
        }

        public virtual async Task<TViewModel> ReadAsync(int id)
        {
            return await ReadAsync<TViewModel>(new EntitySpecification<TEntity>(id));
        }

        public virtual async Task<TViewModel> UpdateAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new BadRequestException("Empty body");
            var entityToUpdate = await _service.ReadSingleAsync(new Specification<TEntity>(e => e.GetHashCode() == viewModel.GetHashCode()));
            var entitiy = await _service.UpdateAsync(viewModel.UpdateModel(entityToUpdate));
            return await GetViewModelAsync<TViewModel>(entitiy);
        }

        public virtual async Task<Response> DeleteAsync(int id)
        {
            return await DeleteAsync(new EntitySpecification<TEntity>(id));
        }

        public virtual async Task<Response> CheckUpdateAuthorizationAsync(int id)
        {
            var entity = await _service.ReadSingleAsync(new EntitySpecification<TEntity>(id));
            await _identityService.AuthorizeAsync(User, OperationAuthorizationRequirements.Update, entity, true);
            return new Response("You are authorized to update");
        }

        protected async Task<IndexViewModel<TCustomViewModel>> IndexAsync<TCustomViewModel>(Specification<TEntity> spec)
            //where TCustomIndexViewModel: EntityIndexViewModel<TCustomViewModel, TEntity>
            where TCustomViewModel : TViewModel
        {
            if (spec.Take > _maxTake || spec.Skip < 0)
            {
                string message = $"Wrong page or page size parameter. Max page size: {_maxTake}";
                throw new BadRequestException(message);
            }
            var entities = await _service.EnumerateAsync(spec);
            int totalCount = await _service.CountTotalAsync(spec);
            int totalPages = await _service.PageCountAsync(spec);
            return await GetIndexViewModelAsync<TCustomViewModel>(spec.Page, totalPages, totalCount, entities);
        }

        protected virtual async Task<IndexViewModel<TViewModel>> IndexAsync(Specification<TEntity> spec)
        {
            return await IndexAsync<TViewModel>(spec);
        }

        protected async Task<IndexViewModel<TViewModel>> IndexByFunctionNotPagedAsync<TCustom>(Func<Specification<TCustom>, Task<IEnumerable<TEntity>>> retrievingFunction, Specification<TCustom> spec)
            where TCustom : class
        {
            return await GetNotPagedIndexViewModelAsync<TViewModel>(await retrievingFunction(spec));
        }

        protected async Task<IndexViewModel<TViewModel>> IndexByRelatedNotPagedAsync<TRelated>
            (Func<Specification<TRelated>, Task<IEnumerable<TEntity>>> retrievingFunction,
            Specification<TRelated> relatedSpec)
            where TRelated : Entity
        {
            return await GetNotPagedIndexViewModelAsync<TViewModel>(await retrievingFunction(relatedSpec));
        }

        protected async Task<IndexViewModel<TViewModel>> IndexNotPagedAsync(Specification<TEntity> spec)
        {
            return await IndexNotPagedAsync<TViewModel>(spec);
        }

        protected async Task<IndexViewModel<TCustomViewModel>> IndexNotPagedAsync<TCustomViewModel>(Specification<TEntity> spec) where TCustomViewModel : TViewModel
        {
            spec.SetPaging(1, _maxTake);
            return await GetNotPagedIndexViewModelAsync<TCustomViewModel>(await _service.EnumerateAsync(spec));
        }

        protected async Task<TCustomViewModel> ReadAsync<TCustomViewModel>(Specification<TEntity> spec) where TCustomViewModel : TViewModel
        {
            return await GetViewModelAsync<TCustomViewModel>(await _service.ReadSingleAsync(spec));
        }

        protected Task<IndexViewModel<TViewModel>> GetNotPagedIndexViewModelAsync(IEnumerable<TEntity> entities)
        {
            return GetNotPagedIndexViewModelAsync<TViewModel>(entities);
        }

        protected async Task<IndexViewModel<TCustomViewModel>> GetNotPagedIndexViewModelAsync<TCustomViewModel>(IEnumerable<TEntity> entities) where TCustomViewModel : TViewModel
        {
            int totalCount = entities.Count();
            int page = totalCount == 0 ? 0 : 1;
            int totalPages = page;
            return await GetIndexViewModelAsync<TCustomViewModel>(page, totalPages, totalCount, entities);
        }

        protected async Task<IndexViewModel<TCustomViewModel>> GetIndexViewModelAsync<TCustomViewModel>(int page, int totalPages, int totalCount, IEnumerable<TEntity> entities)
            where TCustomViewModel : TViewModel
        {
            var viewModels = new List<TCustomViewModel>();
            foreach (var entity in entities)
                viewModels.Add(await GetViewModelAsync<TCustomViewModel>(entity));
            return IndexViewModel<TCustomViewModel>.FromEnumerable(page, totalPages, totalCount, viewModels);
            //var types = new Type[] { typeof(int), typeof(int), typeof(int), typeof(IEnumerable<TEntity>) };
            //return (TCustomIndexViewModel)ast.GetActivator(typeof(TCustomIndexViewModel), types)(page, totalPages, totalCount, entities);
        }

        protected async Task<TCustomViewModel> GetViewModelAsync<TCustomViewModel>(TEntity entity) where TCustomViewModel : TViewModel
        {
            var viewModel = _activatorService.GetInstance<TCustomViewModel>(entity);
            await PopulateViewModelWithRelatedDataAsync(viewModel);
            return viewModel;
        }

        protected virtual Task PopulateViewModelWithRelatedDataAsync<TCustomViewModel>(TCustomViewModel viewModel) where TCustomViewModel : TViewModel => Task.FromResult(viewModel);

        protected virtual async Task<Response> DeleteSingleAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            await _service.DeleteSingleAsync(spec);
            return new Response($"Deleted {_entityName}(s) according to spec {spec}");
        }
        protected virtual HashSet<int> ParseIds(string value)
        {
            var result = new HashSet<int>();
            if (!string.IsNullOrWhiteSpace(value))
            {
                foreach(string idString in value.Split(';'))
                {
                    if (int.TryParse(idString, out int id) && id > 0)
                    {
                        result.Add(id);
                    }
                }
            }
            return result;
        }
        protected virtual async Task<Response> DeleteAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            int deleted = await _service.DeleteAsync(spec);
            return new Response($"Deleted {deleted} {_entityName}(s)");
        }



        protected virtual void ApplyOrderingToSpec(Specification<TEntity> spec, string orderBy, bool? orderByDescending)
        {
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                spec.OrderByDesc = orderByDescending ?? false;
                var values = orderBy.Split(',').Distinct();
                foreach (string value in values)
                {
                    ParseOrderBy(spec, value);
                }
            }
        }

        protected virtual void ParseOrderBy(Specification<TEntity> spec, string value)
        {
            if (value.EqualsCI("Id"))
            {
                spec.OrderByExpressions.Add(e => e.Id);
            }
        }
    }
}
