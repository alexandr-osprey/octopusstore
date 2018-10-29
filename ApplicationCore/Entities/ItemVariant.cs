using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Variant of an Item containing it's Characteristic Values, price and title
    /// </summary>
    public class ItemVariant: Entity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }

        public Item Item { get; set; }
        public ICollection<ItemProperty> ItemProperties { get; set; } = new List<ItemProperty>();
    }
}
