﻿using ApplicationCore.Entities;
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

namespace OctopusStore.Controllers
{
    public class CRUDController<TService, TEntity, TViewModel> : Controller, IController<TEntity, TViewModel>
        where TService : IService<TEntity>
        where TEntity : Entity
        where TViewModel : EntityViewModel<TEntity>
    {
        protected TService Service { get; }
        protected IActivatorService ActivatorService { get; }
        protected IAppLogger<IController<TEntity, TViewModel>> Logger { get; }
        protected string EntityName { get; } = typeof(TEntity).Name;
        protected int MaxTake { get; } = 200;
        protected int DefaultTake { get; } = 50;
        public IScopedParameters ScopedParameters { get; }

        public CRUDController(
            TService service,
            IActivatorService activatorService,
            IScopedParameters scopedParameters,
            IAppLogger<IController<TEntity, TViewModel>> logger)
        {
            Service = service;
            ActivatorService = activatorService;
            ScopedParameters = scopedParameters;
            Logger = logger;
            Logger.Name = EntityName + "Controller";
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
            var entity = await Service.CreateAsync(viewModel.ToModel());
            return GetViewModel<TViewModel>(entity);
        }

        public virtual async Task<TViewModel> ReadAsync(int id)
        {
            return await ReadAsync<TViewModel>(new EntitySpecification<TEntity>(id));
        }

        public virtual async Task<TViewModel> UpdateAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new BadRequestException("Empty body");
            var entityToUpdate = await Service.ReadSingleAsync(new Specification<TEntity>(e => e.GetHashCode() == viewModel.GetHashCode()));
            var entitiy = await Service.UpdateAsync(viewModel.UpdateModel(entityToUpdate));
            return GetViewModel<TViewModel>(entitiy);
        }

        public virtual async Task<Response> DeleteAsync(int id)
        {
            return await DeleteAsync(new EntitySpecification<TEntity>(id));
        }

        public virtual async Task<Response> CheckUpdateAuthorizationAsync(int id)
        {
            var entity = await Service.ReadSingleAsync(new EntitySpecification<TEntity>(id));
            await Service.IdentityService.AuthorizeAsync(User, entity, OperationAuthorizationRequirements.Update, true);
            return new Response("You are authorized to update");
        }

        protected async Task<IndexViewModel<TCustomViewModel>> IndexAsync<TCustomViewModel>(Specification<TEntity> spec)
            //where TCustomIndexViewModel: EntityIndexViewModel<TCustomViewModel, TEntity>
            where TCustomViewModel : EntityViewModel<TEntity>
        {
            if (spec.Take > MaxTake || spec.Skip < 0)
            {
                string message = $"Wrong page or page size parameter. Max page size: {MaxTake}";
                throw new BadRequestException(message);
            }
            var entities = await Service.EnumerateAsync(spec);
            int totalCount = await Service.CountTotalAsync(spec);
            int totalPages = await Service.PageCountAsync(spec);
            return GetIndexViewModel<TCustomViewModel>(spec.Page, totalPages, totalCount, entities);
        }

        protected virtual async Task<IndexViewModel<TViewModel>> IndexAsync(Specification<TEntity> spec)
        {
            return await IndexAsync<TViewModel>(spec);
        }

        protected async Task<IndexViewModel<TViewModel>> IndexByFunctionNotPagedAsync<TCustom>(Func<Specification<TCustom>, Task<IEnumerable<TEntity>>> retrievingFunction, Specification<TCustom> spec)
            where TCustom : class
        {
            return GetNotPagedIndexViewModel<TViewModel>(await retrievingFunction(spec));
        }

        protected async Task<IndexViewModel<TViewModel>> IndexByRelatedNotPagedAsync<TRelated>
            (Func<Specification<TRelated>, Task<IEnumerable<TEntity>>> retrievingFunction,
            Specification<TRelated> relatedSpec)
            where TRelated : Entity
        {
            return GetNotPagedIndexViewModel<TViewModel>(await retrievingFunction(relatedSpec));
        }

        protected async Task<IndexViewModel<TViewModel>> IndexNotPagedAsync(Specification<TEntity> spec)
        {
            return await IndexNotPagedAsync<TViewModel>(spec);
        }

        protected async Task<IndexViewModel<TCustomViewModel>> IndexNotPagedAsync<TCustomViewModel>(Specification<TEntity> spec) where TCustomViewModel : EntityViewModel<TEntity>
        {
            spec.SetPaging(1, MaxTake);
            return GetNotPagedIndexViewModel<TCustomViewModel>(await Service.EnumerateAsync(spec));
        }

        protected async Task<TDetailViewModel> ReadDetailAsync<TDetailViewModel>(Specification<TEntity> spec) where TDetailViewModel : EntityViewModel<TEntity>
        {
            return GetViewModel<TDetailViewModel>(await Service.ReadSingleAsync(spec));
        }

        protected async Task<TCustomViewModel> ReadAsync<TCustomViewModel>(Specification<TEntity> spec) where TCustomViewModel : EntityViewModel<TEntity>
        {
            return GetViewModel<TCustomViewModel>(await Service.ReadSingleAsync(spec));
        }

        protected IndexViewModel<TViewModel> GetNotPagedIndexViewModel(IEnumerable<TEntity> entities)
        {
            return GetNotPagedIndexViewModel<TViewModel>(entities);
        }

        protected IndexViewModel<TCustomViewModel> GetNotPagedIndexViewModel<TCustomViewModel>(IEnumerable<TEntity> entities) where TCustomViewModel : EntityViewModel<TEntity>
        {
            int totalCount = entities.Count();
            int page = totalCount == 0 ? 0 : 1;
            int totalPages = page;
            return GetIndexViewModel<TCustomViewModel>(page, totalPages, totalCount, entities);
        }

        protected IndexViewModel<TCustomViewModel> GetIndexViewModel<TCustomViewModel>(int page, int totalPages, int totalCount, IEnumerable<TEntity> entities)
            where TCustomViewModel : EntityViewModel<TEntity>
        {
            var viewModels = from e in entities select GetViewModel<TCustomViewModel>(e);
            return IndexViewModel<TCustomViewModel>.FromEnumerable(page, totalPages, totalCount, viewModels);
            //var types = new Type[] { typeof(int), typeof(int), typeof(int), typeof(IEnumerable<TEntity>) };
            //return (TCustomIndexViewModel)ast.GetActivator(typeof(TCustomIndexViewModel), types)(page, totalPages, totalCount, entities);
        }

        protected TCustomViewModel GetViewModel<TCustomViewModel>(TEntity entity) where TCustomViewModel : EntityViewModel<TEntity>
        {
            //return (TCustomViewModel)new ActivatorsStorage().GetActivator(typeof(TCustomViewModel), typeof(TEntity))(entity);
            return ActivatorService.GetInstance<TCustomViewModel>(entity);
        }

        protected virtual async Task<Response> DeleteSingleAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            await Service.DeleteSingleAsync(spec);
            return new Response($"Deleted {EntityName}(s) according to spec {spec}");
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
            int deleted = await Service.DeleteAsync(spec);
            return new Response($"Deleted {deleted} {EntityName}(s)");
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
