using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Variant of an Item containing it's Characteristic Values, price and title
    /// </summary>
    public class ItemVariant : Entity, IGenericMemberwiseClonable<ItemVariant>
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
        public virtual ICollection<ItemProperty> ItemProperties { get; set; } = new List<ItemProperty>();

        public ItemVariant() : base()
        {
        }

        public bool Equals(ItemVariant other) => base.Equals(other)
            && Title.EqualsCI(other.Title)
            && Price == other.Price
            && ItemId == other.ItemId;
        public override bool Equals(object obj) => Equals(obj as ItemVariant);
        public override int GetHashCode() => base.GetHashCode();

        protected ItemVariant(ItemVariant itemVariant) : base(itemVariant)
        {
            Title = itemVariant.Title;
            Price = itemVariant.Price;
            ItemId = itemVariant.ItemId;
        }

        public ItemVariant ShallowClone()
        {
            return (ItemVariant)MemberwiseClone();
        }
    }
}
