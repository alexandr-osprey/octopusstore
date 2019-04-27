using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantDetailViewModel: ItemVariantThumbnailViewModel
    {
        public IEnumerable<ItemPropertyViewModel> ItemProperties { get; set; }

        public ItemVariantDetailViewModel(ItemVariant itemVariant): base(itemVariant)
        {
            ItemProperties = itemVariant.ItemProperties.Select(propertyValue => new ItemPropertyViewModel(propertyValue));
        }
    }
}
