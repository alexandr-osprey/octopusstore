using System;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using ApplicationCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace OctopusStore.Controllers
{
    public class ReadWriteController<TService, TEntity, TViewModel, TDetailViewModel, TIndexViewModel> 
        : ReadController<TService, TEntity, TViewModel, TDetailViewModel, TIndexViewModel>
        where TService : IService<TEntity> where TEntity : Entity
        where TViewModel : ViewModel<TEntity>, new()
        where TDetailViewModel : DetailViewModel<TEntity>
        where TIndexViewModel : IndexViewModel<TViewModel, TEntity>
    {
        public ReadWriteController(
            TService serivce, 
            IAppLogger<IEntityController<TEntity>> logger)
            : base(serivce, logger)
        {  }

        protected async Task<TViewModel> PostAsync(TViewModel viewModel)
        {
            return await CreateOrUpdate<Specification<TEntity>>(viewModel.ToModel(), _serivce.AddAsync);
        }
        protected async Task<TViewModel> PutAsync(TViewModel viewModel)
        {
            if (viewModel == null)
                throw new EntityValidationError($"{_entityName} is empty");
            var spec = new Specification<TEntity>(viewModel.Id);
            if (!_serivce.Exist(spec))
                throw new EntityNotFoundException($"{_entityName} with spec {spec} does not exist. ");
            try
            {
                var entity = await _serivce.GetSingleAsync(new Specification<TEntity>(viewModel.Id));
                viewModel.UpdateModel(entity);
                return await CreateOrUpdate<Specification<TEntity>>(entity, _serivce.UpdateAsync);
            }
            catch (Exception exception)
            {
                string message = $"Error retreiving {_entityName} with id {viewModel.Id}";
                _logger.Warn(exception, message);
                throw new EntityValidationError(message);
            }
            
        }
        protected async Task<ActionResult> DeleteAsync(ISpecification<TEntity> spec)
        {
            if (!_serivce.Exist(spec))
            {
                throw new EntityNotFoundException($"{_entityName} with spec {spec} does not exist. ");
            }
            try
            {
                await _serivce.DeleteAsync(spec);
                return Ok(new Response($"{_entityName} with spec {spec} deleted"));
            }
            catch (Exception exception)
            {
                string message = $"Error deleting {_entityName} with spec {spec}";
                _logger.Warn(exception, message);
                throw new InternalServerException(message);
            }
        }
        async private Task<TViewModel> CreateOrUpdate<TSpecification>(TEntity  entity, Func<TEntity, Task<TEntity>> saveFunction) where TSpecification: ISpecification<TEntity>
        {
            if (entity == null)
                throw new EntityValidationError($"{_entityName} is empty");
            try
            {
                await saveFunction(entity);
                var spec = (TSpecification)Activator.CreateInstance(typeof(TSpecification), entity.Id);
                var entityDetail = await _serivce.GetSingleAsync(spec);
                return (TViewModel)Activator.CreateInstance(typeof(TViewModel), entityDetail);
            }
            catch (Exception exception)
            {
                string message = $"Error creating or updating entity: {entity}";
                _logger.Warn(exception, message);
                throw new EntityValidationError(message);
            }
        }
    }
}
