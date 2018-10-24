using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class StoreIndexViewModel : EntityIndexViewModel<StoreViewModel, Store>
    {
        public StoreIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Store> stores)
            : base(page, totalPages, totalCount, from store in stores select new StoreViewModel(store))
        {
        }
    }
}
