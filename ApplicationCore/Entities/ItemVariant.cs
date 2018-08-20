using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ItemVariant : Entity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }
        public ICollection<ItemVariantCharacteristicValue> ItemVariantCharacteristicValues { get; set; } = new List<ItemVariantCharacteristicValue>();
    }
}
