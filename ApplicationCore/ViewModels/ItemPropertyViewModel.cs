using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class ItemPropertyViewModel: EntityViewModel<ItemProperty>
    {
        public int ItemVariantId { get; set; }
        public int CharacteristicValueId { get; set; }

        public ItemPropertyViewModel()
           : base()
        {
        }
        public ItemPropertyViewModel(ItemProperty itemProperty)
           : base(itemProperty)
        {
            Id = itemProperty.Id;
            ItemVariantId = itemProperty.ItemVariantId;
            CharacteristicValueId = itemProperty.CharacteristicValueId;
        }

        public override ItemProperty ToModel()
        {
            return new ItemProperty()
            {
                Id = Id,
                ItemVariantId = ItemVariantId,
                CharacteristicValueId = CharacteristicValueId,
            };
        }
        public override ItemProperty UpdateModel(ItemProperty modelToUpdate)
        {
            //modelToUpdate.CharacteristicValueId = CharacteristicValueId;
            return modelToUpdate;
        }
    }
}
