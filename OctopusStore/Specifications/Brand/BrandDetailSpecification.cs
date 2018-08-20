using ApplicationCore.Entities;

namespace OctopusStore.Specifications
{
    public class BrandDetailSpecification : DetailSpecification<Brand>
    {
        public BrandDetailSpecification(int id)
            : base(id)
        { }
    }
}
