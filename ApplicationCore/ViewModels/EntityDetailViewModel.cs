using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    /// <summary>
    /// View Model for details of PrimalEntity. Supposed to have entity and related fields to avoid excessive querying.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityDetailViewModel<T> : EntityViewModel<T> where T : Entity
    {
        public EntityDetailViewModel(T entity) : base(entity)
        {
        }
    }
}
