using ApplicationCore.Entities;

namespace OctopusStore.Specifications
{
    public class ItemVariantCharacteristicValueDetailSpecification : DetailSpecification<ItemVariantCharacteristicValue>
    {
        public ItemVariantCharacteristicValueDetailSpecification(int id)
            : base(id)
        {
            AddInclude("CharacteristicValue.Characteristic");
        }
    }
}
