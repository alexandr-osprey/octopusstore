using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemThumbnailViewModel: ItemViewModel
    {
        public IEnumerable<ItemImageViewModel> Images { get; set; }
        public BrandViewModel Brand { get; set; }
        public IEnumerable<ItemVariantViewModel> ItemVariants { get; set; }

        public ItemThumbnailViewModel(): base()
        {
        }

        public ItemThumbnailViewModel(Item item): base(item)
        {
            Images = item.Images.Select(i => new ItemImageViewModel(i));
            ItemVariants = item.ItemVariants.Select(v => new ItemVariantViewModel(v));
            Brand = new BrandViewModel(item.Brand);
        }
    }
}
