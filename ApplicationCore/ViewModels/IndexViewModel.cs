using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public abstract class IndexViewModel<TViewModel, TEntity> where TViewModel : ViewModel<TEntity> where TEntity : Entity
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<TViewModel> Entities { get; set; }

        public IndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> entities)
        {
            Page = page >= 0 ? page : 0;
            TotalPages = totalPages;
            TotalCount = totalCount;
            PageSize = entities.Count();
            Entities = entities;
        }
    }
}
