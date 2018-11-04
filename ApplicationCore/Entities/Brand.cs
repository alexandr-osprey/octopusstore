using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Basic information about a Brand
    /// </summary>
    public class Brand: Entity, IGenericMemberwiseClonable<Brand>
    {
        public string Title { get; set; }
        public IEnumerable<Item> Items = new List<Item>();

        public Brand(): base()
        {
        }

        protected Brand(Brand brand): base(brand)
        {
            Title = brand.Title;
        }

        public Brand ShallowClone()
        {
            return (Brand)MemberwiseClone();
        }
    }
}
