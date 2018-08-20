using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public abstract class DetailViewModel<T> : ViewModel<T> where T : Entity
    {
        public string Title { get; set; }

        public DetailViewModel(T entity)
            : base(entity)
        {  }
    }
}
