using ApplicationCore.Entities;

namespace OctopusStore.Specifications
{
    public class ItemImageDetailSpecification : FileDetailSpecification<ItemImage, Item>
    {
        public ItemImageDetailSpecification(int id)
            : base(id)
        {  }
    }
}
