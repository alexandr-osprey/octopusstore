using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class CategoryIndexViewModel : IndexViewModel<CategoryViewModel, Category>
    {
        public CategoryIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Category> categories)
            : base(page, totalPages, totalCount, from category in categories select new CategoryViewModel(category))
        {   }
    }
}
