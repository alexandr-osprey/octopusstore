using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    /// <summary>
    /// View model for a PrimalEntity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EntityViewModel<T> where T : Entity
    {
        /// <summary>
        /// Primal key of an entity
        /// </summary>
        public int Id { get; set; }

        public EntityViewModel()
        {
        }
        public EntityViewModel(T entity)
        {
            Id = entity.Id;
        }


        /// <summary>
        /// Constructs a new entity from view model
        /// </summary>
        /// <returns></returns>
        public abstract T ToModel();
        /// <summary>
        /// Updates specified entity with data from view model. For database consistency not all fields are supposed to be updated, but only ones which are safe to do so.
        /// </summary>
        /// <param name="modelToUpdate"></param>
        public abstract T UpdateModel(T modelToUpdate);

        public override string ToString() => (new { Type = GetType().Name, Id }).ToString();
    }
}
