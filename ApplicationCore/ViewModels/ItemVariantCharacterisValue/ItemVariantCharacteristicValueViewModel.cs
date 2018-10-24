using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantCharacteristicValueViewModel : EntityViewModel<ItemVariantCharacteristicValue>
    {
        public int ItemVariantId { get; set; }
        public int CharacteristicValueId { get; set; }

        public ItemVariantCharacteristicValueViewModel()
            : base()
        {
        }
        public ItemVariantCharacteristicValueViewModel(ItemVariantCharacteristicValue itemVariantPropertyValue)
            : base(itemVariantPropertyValue)
        {
            Id = itemVariantPropertyValue.Id;
            ItemVariantId = itemVariantPropertyValue.ItemVariantId;
            CharacteristicValueId = itemVariantPropertyValue.CharacteristicValueId;
        }

        public override ItemVariantCharacteristicValue ToModel()
        {
            return new ItemVariantCharacteristicValue()
            {
                Id = Id,
                ItemVariantId = ItemVariantId,
                CharacteristicValueId = CharacteristicValueId,
            };
        }
        public override ItemVariantCharacteristicValue UpdateModel(ItemVariantCharacteristicValue modelToUpdate)
        {
            modelToUpdate.CharacteristicValueId = CharacteristicValueId;
            return modelToUpdate;
        }
    }
}
