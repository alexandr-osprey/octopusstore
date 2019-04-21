using ApplicationCore.Extensions;
using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Basic information about a Brand
    /// </summary>
    public class Brand: Entity, ShallowClonable<Brand>
    {
        public string Title { get; set; }

        public virtual IEnumerable<Item> Items { get; set; } = new List<Item>();

        public Brand(): base()
        {
        }

        public bool Equals(Brand other) => base.Equals(other) 
            && Title.EqualsCI(other.Title);
        public override bool Equals(object obj) => Equals(obj as Brand);
        public override int GetHashCode() => base.GetHashCode();

        protected Brand(Brand brand): base(brand)
        {
            Title = brand.Title;
        }

        public Brand ShallowClone()
        {
            return new Brand(this);
        }
    }
}
