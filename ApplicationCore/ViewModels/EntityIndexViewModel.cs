using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    /// <summary>
    /// View model for multiple entities with paging information
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityIndexViewModel<TViewModel, TEntity>: IndexViewModel<TViewModel> where TViewModel: EntityViewModel<TEntity>  where TEntity: Entity
    {
        public EntityIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> viewModels)
           : base(page, totalPages, totalCount, viewModels)
        {
            Entities = viewModels.OrderBy(e => e.Id);
        }
    }
}
