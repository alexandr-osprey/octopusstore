using ApplicationCore.Entities;
using ApplicationCore.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace OctopusStore.ViewModels
{
    public class ItemVariantDetailViewModel : DetailViewModel<ItemVariant>
    {
        public int ItemId { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<ItemVariantCharacteristicValueViewModel> ItemVariantCharacteristicValues { get; set; }

        public ItemVariantDetailViewModel(ItemVariant itemVariant)
            : base(itemVariant)
        {
            Id = itemVariant.Id;
            Title = itemVariant.Title;
            ItemId = itemVariant.ItemId;
            Price = itemVariant.Price;
            ItemVariantCharacteristicValues = (from propertyValue in itemVariant.ItemVariantCharacteristicValues
                                            select new ItemVariantCharacteristicValueViewModel(propertyValue));
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
        public override void UpdateModel(ItemVariant modelToUpdate)
        {
            modelToUpdate.Price = Price;
            modelToUpdate.Title = Title;
        }
    }
}
