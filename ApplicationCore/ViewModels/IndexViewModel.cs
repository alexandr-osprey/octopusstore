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

        public IEnumerable<TViewModel> Entities { get; set; } = new List<TViewModel>();

        public IndexViewModel()
        {
        }

        public IndexViewModel(int page, int totalPages, int totalCount, IEnumerable<TViewModel> viewModels)
        {
            Page = page >= 0 ? page: 0;
            TotalPages = totalPages;
            TotalCount = totalCount;
            PageSize = viewModels.Count();
            Entities = viewModels;
        }

        public bool Equals(IndexViewModel<TViewModel> other)
        {
            bool result = null != other
            && Page == other.Page
            && PageSize == other.PageSize
            && TotalPages == other.TotalPages
            && TotalCount == other.TotalCount
            && Entities.Count() == other.Entities.Count();
            if (result)
            {
                int count = Entities.Count();
                for (int i = 0; i < count; i++)
                    if (!(result &= Entities.ElementAt(i).Equals(other.Entities.ElementAt(i))))
                        break;
            }
            return result;
        }

        public override bool Equals(object obj) => Equals(obj as IndexViewModel<TViewModel>);
        public override int GetHashCode() => Page * 111 + PageSize * 97 + TotalPages * 199 + TotalCount * 911 + (from e in Entities select e.GetHashCode()^2).Sum();

        public static IndexViewModel<TViewModel> FromEnumerableNotPaged(IEnumerable<TViewModel> viewModels)
        {
            bool isEmpty = !viewModels.Any();
            return FromEnumerable(isEmpty ? 0: 1, isEmpty ? 0: 1, viewModels.Count(), viewModels);
        }
        public static IndexViewModel<TViewModel> FromEnumerable(int page, int totalPages, int totalCount, IEnumerable<TViewModel> viewModels)
        {
            return new IndexViewModel<TViewModel>(page, totalPages, totalCount, viewModels);
        }
    }
}
