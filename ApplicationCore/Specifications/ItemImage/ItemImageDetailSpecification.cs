using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public class ItemImageDetailSpecification : FileDetailSpecification<ItemImage, Item>
    {
        public ItemImageDetailSpecification(int id)
            : base(id)
        {  }
    }
}
