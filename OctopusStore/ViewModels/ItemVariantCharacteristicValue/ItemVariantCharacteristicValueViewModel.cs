using ApplicationCore.Entities;
using ApplicationCore.ViewModels;

namespace OctopusStore.ViewModels
{
    public class ItemVariantCharacteristicValueViewModel : ViewModel<ItemVariantCharacteristicValue>
    {
        public int ItemVariantId { get; set; }
        public int CharacteristicValueId { get; set; }

        public ItemVariantCharacteristicValueViewModel()
            : base()
        { }
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
        public override void UpdateModel(ItemVariantCharacteristicValue modelToUpdate)
        {
            modelToUpdate.CharacteristicValueId = CharacteristicValueId;
        }
    }
}
