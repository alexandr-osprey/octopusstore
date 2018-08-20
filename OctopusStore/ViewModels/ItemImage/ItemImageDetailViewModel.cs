using ApplicationCore.Entities;

namespace OctopusStore.ViewModels
{
    public class ItemImageDetailViewModel : ImageDetailViewModel<ItemImage, Item>
    {
        public ItemImageDetailViewModel(ItemImage image)
            : base(image)
        { }

        public override ItemImage ToModel()
        {
            var image = new ItemImage(OwnerUsername,  ContentType, RelatedId);
            image.Id = Id;
            return image;
        }
    }
}
