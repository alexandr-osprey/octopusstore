using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Item for selling
    /// </summary>
    public class Item: Entity, IGenericMemberwiseClonable<Item>
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public int BrandId { get; set; }
        public int MeasurementUnitId { get; set; }
        public string Description { get; set; }

        public virtual Category Category { get; set; }
        public virtual Store Store { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
        public virtual ICollection<ItemImage> Images { get; set; } = new List<ItemImage>();
        public virtual ICollection<ItemVariant> ItemVariants { get; set; } = new List<ItemVariant>();

        public Item(): base()
        {
        }

        public bool Equals(Item other) => base.Equals(other)
            && Title.EqualsCI(other.Title)
            && CategoryId == other.CategoryId
            && StoreId == other.StoreId
            && BrandId == other.BrandId
            && MeasurementUnitId == other.MeasurementUnitId
            && Description.EqualsCI(other.Description);
        public override bool Equals(object obj) => Equals(obj as Item);
        public override int GetHashCode() => base.GetHashCode();

        protected Item(Item item): base(item)
        {
            Id = 0;
            Title = item.Title;
            CategoryId = item.CategoryId;
            StoreId = item.StoreId;
            BrandId = item.BrandId;
            MeasurementUnitId = item.MeasurementUnitId;
            Description = item.Description;
        }

        public Item ShallowClone()
        {
            return new Item(this);
        }
    }
}
