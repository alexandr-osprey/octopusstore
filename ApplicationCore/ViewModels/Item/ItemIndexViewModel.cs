using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemIndexViewModel : EntityIndexViewModel<ItemViewModel, Item>
    {
        public ItemIndexViewModel(int page, int totalPages, int totalCount, IEnumerable<Item> items)
            : base(page, totalPages, totalCount, from item in items select new ItemViewModel(item))
        {
        }
    }
}
