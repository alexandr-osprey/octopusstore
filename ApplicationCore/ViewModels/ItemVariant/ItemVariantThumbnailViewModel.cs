using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantThumbnailViewModel : ItemVariantViewModel
    {
        public IEnumerable<ItemVariantImageViewModel> Images { get; set; }

        public ItemVariantThumbnailViewModel(ItemVariant itemVariant): base(itemVariant)
        {
            Images = itemVariant.Images.Select(i => new ItemVariantImageViewModel(i));
        }
    }
}
