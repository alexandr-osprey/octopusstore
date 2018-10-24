using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantCharacteristicValueDetailViewModel : EntityDetailViewModel<ItemVariantCharacteristicValue>
    {
        public int ItemVariantId { get; set; }
        public CharacteristicViewModel Characteristic { get; set; }
        public CharacteristicValueViewModel CharacteristicValue { get; set; }

        public ItemVariantCharacteristicValueDetailViewModel(ItemVariantCharacteristicValue itemVariantPropertyValue)
            : base(itemVariantPropertyValue)
        {
            ItemVariantId = itemVariantPropertyValue.ItemVariantId;
            Characteristic = new CharacteristicViewModel(itemVariantPropertyValue.CharacteristicValue.Characteristic);
            CharacteristicValue = new CharacteristicValueViewModel(itemVariantPropertyValue.CharacteristicValue);
        }

        public override ItemVariantCharacteristicValue ToModel()
        {
            return new ItemVariantCharacteristicValue()
            {
                Id = Id,
                ItemVariantId = ItemVariantId,
                CharacteristicValueId = CharacteristicValue.Id,
            };
        }
        public override ItemVariantCharacteristicValue UpdateModel(ItemVariantCharacteristicValue modelToUpdate)
        {
            modelToUpdate.CharacteristicValueId = CharacteristicValue.Id;
            return modelToUpdate;
        }
    }
}
