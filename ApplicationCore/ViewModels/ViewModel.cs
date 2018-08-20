using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public abstract class ViewModel<T> where T : Entity
    {
        public int Id { get; set; }

        public ViewModel()
        { }
        public ViewModel(T entity)
        {
            Id = entity.Id;
        }

        public abstract T ToModel();
        public abstract void UpdateModel(T modelToUpdate);
    }
}
