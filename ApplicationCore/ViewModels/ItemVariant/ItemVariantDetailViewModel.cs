using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantDetailViewModel: ItemVariantViewModel
    {
        public IEnumerable<ItemPropertyViewModel> ItemProperties { get; set; }
        public IEnumerable<ItemVariantImageViewModel> Images { get; set; }

        public ItemVariantDetailViewModel(ItemVariant itemVariant): base(itemVariant)
        {
            ItemProperties = itemVariant.ItemProperties.Select(propertyValue => new ItemPropertyViewModel(propertyValue));
            Images = itemVariant.Images.Select(i => new ItemVariantImageViewModel(i));
        }
    }
}
