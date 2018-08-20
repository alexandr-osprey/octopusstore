using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class ItemImageIndexViewModel : ImageIndexViewModel<ItemImage, Item, ItemImageViewModel>
    {
        public ItemImageIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<ItemImage> itemImages)
            : base(page, totalPages, totalCount, from itemImage in itemImages select new ItemImageViewModel(itemImage))
        {  }
    }
}
