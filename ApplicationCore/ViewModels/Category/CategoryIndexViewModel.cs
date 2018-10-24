using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class CategoryIndexViewModel : EntityIndexViewModel<CategoryViewModel, Category>
    {
        public CategoryIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Category> categories)
            : base(page, totalPages, totalCount, from category in categories select new CategoryViewModel(category))
        {
        }
    }
}
