using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class ItemVariantCharacteristicValueDetailViewModel : DetailViewModel<ItemVariantCharacteristicValue>
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
        public override void UpdateModel(ItemVariantCharacteristicValue modelToUpdate)
        {
            modelToUpdate.CharacteristicValueId = CharacteristicValue.Id;
        }
    }
}
