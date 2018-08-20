using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class BrandIndexViewModel : IndexViewModel<BrandViewModel, Brand>
    {
        public BrandIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Brand> brands)
            : base(page, totalPages, totalCount, from brand in brands select new BrandViewModel(brand))
        {  }
    }
}
