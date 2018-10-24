using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class StoreDetailSpecification : DetailSpecification<Store>
    {
        public StoreDetailSpecification(int id)
            : base(id)
        {
        }
    }
}
