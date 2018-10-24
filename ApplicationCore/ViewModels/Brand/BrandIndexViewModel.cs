using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class BrandIndexViewModel : EntityIndexViewModel<BrandViewModel, Brand>
    {
        public BrandIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Brand> brands)
            : base(page, totalPages, totalCount, from brand in brands select new BrandViewModel(brand))
        {
        }
    }
}
