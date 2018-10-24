using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Variant of an Item containing it's Characteristic Values, price and title
    /// </summary>
    public class ItemVariant : Entity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }
        public ICollection<ItemVariantCharacteristicValue> ItemVariantCharacteristicValues { get; set; } = new List<ItemVariantCharacteristicValue>();
    }
}
