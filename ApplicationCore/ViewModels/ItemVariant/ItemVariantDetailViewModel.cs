using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantDetailViewModel: EntityViewModel<ItemVariant>
    {
        public string Title { get; set; }
        public int ItemId { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ItemPropertyViewModel> ItemProperties { get; set; }

        public ItemVariantDetailViewModel(ItemVariant itemVariant)
           : base(itemVariant)
        {
            Id = itemVariant.Id;
            Title = itemVariant.Title;
            ItemId = itemVariant.ItemId;
            Price = itemVariant.Price;
            ItemProperties = (from propertyValue in itemVariant.ItemProperties
                                            select new ItemPropertyViewModel(propertyValue));
        }

        public override ItemVariant ToModel()
        {
            return new ItemVariant()
            {
                Id = Id,
                Price = Price,
                Title = Title,
                ItemId = ItemId
            };
        }
        public override ItemVariant UpdateModel(ItemVariant modelToUpdate)
        {
            modelToUpdate.Price = Price;
            modelToUpdate.Title = Title;
            return modelToUpdate;
        }
    }
}
