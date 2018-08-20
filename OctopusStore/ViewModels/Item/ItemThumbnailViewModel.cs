using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class ItemThumbnailViewModel : ItemViewModel
    {
        public IEnumerable<decimal> Prices { get; set; }
        public IEnumerable<ItemImageViewModel> Images { get; set; }

        public ItemThumbnailViewModel()
            : base()
        { }
        public ItemThumbnailViewModel(Item item)
            : base(item)
        {
            Images = from image in item.Images select new ItemImageViewModel(image);
            Prices = from variant in item.ItemVariants select variant.Price;
        }
    }
}
