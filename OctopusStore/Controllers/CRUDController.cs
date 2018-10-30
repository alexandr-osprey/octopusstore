using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OctopusStore.Controllers
{
    public class CRUDController<TService, TEntity, TViewModel>: Controller
        where TService: IService<TEntity>
        where TEntity: Entity
        where TViewModel: EntityViewModel<TEntity>
    {
        protected TService _service;
        protected readonly IAppLogger<ICRUDController<TEntity>> _logger;
        protected string _entityName = typeof(TEntity).Name;
        protected int _maxTake = 200;
        protected int _defaultTake = 50;
        protected IScopedParameters _scopedParameters;

        public CRUDController(
            TService service,
            IScopedParameters scopedParameters,
            IAppLogger<ICRUDController<TEntity>> logger)
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

        protected async Task<IndexViewModel<TCustomViewModel>> IndexAsync<TCustomViewModel>(Specification<TEntity> spec)
            //where TCustomIndexViewModel: EntityIndexViewModel<TCustomViewModel, TEntity>
            where TCustomViewModel: EntityViewModel<TEntity>
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
        protected async Task<ActionResult> CheckUpdateAuthorizationAsync(int id)
        {
            var entity = await _service.ReadSingleAsync(new EntitySpecification<TEntity>(id));
            await _service.IdentityService.AuthorizeAsync(User, entity, OperationAuthorizationRequirements.Update, true);
            return Ok(new Response("You are authorized to update"));
        }
        protected async Task<TViewModel> UpdateAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new BadRequestException("Empty body");
            var entityToUpdate = await _service.ReadSingleAsync(viewModel.ToModel());
            var entitiy = await _service.UpdateAsync(viewModel.UpdateModel(entityToUpdate));
            return GetViewModel<TViewModel>(entitiy);
        }

        protected async Task<Response> DeleteSingleAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            await _service.DeleteSingleAsync(spec);
            return new Response($"Deleted {_entityName}(s) according to spec {spec}");
        }

        protected async Task<Response> DeleteAsync(Specification<TEntity> spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));
            int deleted = await _service.DeleteAsync(spec);
            return new Response($"Deleted {deleted} {_entityName}(s)");
        }

        protected async Task<IndexViewModel<TViewModel>> IndexAsync(Specification<TEntity> spec)
        {
            return await IndexAsync<TViewModel>(spec);
        }
        protected async Task<IndexViewModel<TViewModel>> IndexByFunctionNotPagedAsync<TCustom>(Func<Specification<TCustom>, Task<IEnumerable<TEntity>>> retrievingFunction, Specification<TCustom> spec)
            where TCustom: class
        {
            return GetNotPagedIndexViewModel(await retrievingFunction(spec));
        }
        protected async Task<IndexViewModel<TViewModel>> IndexByRelatedNotPagedAsync<TRelated>
            (Func<Specification<TRelated>, Task<IEnumerable<TEntity>>> retrievingFunction,
            Specification<TRelated> relatedSpec)
            where TRelated: Entity
        {
            return GetNotPagedIndexViewModel(await retrievingFunction(relatedSpec));
        }
        protected async Task<IndexViewModel<TViewModel>> IndexNotPagedAsync(Specification<TEntity> spec)
        {
            spec.SetPaging(1, _maxTake);
            var entities = await _service.EnumerateAsync(spec);
            return GetNotPagedIndexViewModel(entities);
        }

        protected async Task<TDetailViewModel> GetDetailAsync<TDetailViewModel>(Specification<TEntity> spec) where TDetailViewModel: EntityViewModel<TEntity>
        {
            var entityDetail = await _service.ReadSingleAsync(spec);
            return GetViewModel<TDetailViewModel>(entityDetail);
        }
        protected async Task<TViewModel> GetAsync(Specification<TEntity> spec)
        {
            var entity = await _service.ReadSingleAsync(spec);
            return GetViewModel<TViewModel>(entity);
        }

        protected IndexViewModel<TViewModel> GetNotPagedIndexViewModel(IEnumerable<TEntity> entities)
        {
            int totalCount = entities.Count();
            int page = totalCount == 0 ? 0: 1;
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
            ActivatorsStorage ast = new ActivatorsStorage();
            return (TCustomViewModel)ast.GetActivator(typeof(TCustomViewModel), typeof(TEntity))(entity);
        }
    }

    public class ActivatorsStorage
    {
        public delegate object ObjectActivator(params object[] args);
        protected readonly static Dictionary<Type, ObjectActivator> activators = new Dictionary<Type, ObjectActivator>();

        private ObjectActivator CreateActivator(Type type, params Type[] argTypes)
        {
            if (type == null)
                throw new ArgumentException("Incorrect class name", "className");
            // Get the public instance constructor that takes an integer parameter.
            ConstructorInfo ctor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public, null,
                CallingConventions.HasThis, argTypes, null);
            if (ctor == null)
                throw new Exception("There is no any constructor with specified parameters.");
            //Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
            Expression[] argsExp = new Expression[paramsInfo.Length];
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;
                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }
            NewExpression newExp = Expression.New(ctor, argsExp);
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);
            return (ObjectActivator)lambda.Compile();
        }
        public ObjectActivator GetActivator(Type type, params Type[] argTypes)
        {

            if (activators.TryGetValue(type, out ObjectActivator activator))
            {
                return activator;
            }
            activator = CreateActivator(type, argTypes);
            activators[type] = activator;
            return activator;
        }
    }
}
