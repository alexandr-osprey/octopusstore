using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    /// <summary>
    /// View model for multiple entities with paging information
    /// </summary>
    /// <typeparam name="TViewModel"></typeparam>
    public class IndexViewModel<TViewModel> where TViewModel: class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public IEnumerable<TViewModel> Entities { get; set; }

        public IndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> viewModels)
        {
            Page = page >= 0 ? page : 0;
            TotalPages = totalPages;
            TotalCount = totalCount;
            PageSize = viewModels.Count();
            Entities = viewModels;
        }

        public static IndexViewModel<TViewModel> FromEnumerable(IEnumerable<TViewModel> viewModels)
        {
            bool isEmpty = !viewModels.Any();
            return new IndexViewModel<TViewModel>(isEmpty ? 0 : 1, isEmpty ? 0 : 1, viewModels.Count(), viewModels);
        }
    }
}
