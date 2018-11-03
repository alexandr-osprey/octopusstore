using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Basic information about a Brand
    /// </summary>
    public class Brand: Entity, IGenericMemberwiseClonable<Brand>
    {
        public string Title { get; set; }

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
