using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class ItemImageDetailViewModel : ImageDetailViewModel<ItemImage, Item>
    {
        public ItemImageDetailViewModel(ItemImage image)
            : base(image)
        {
        }

        public override ItemImage ToModel()
        {
            var image = new ItemImage(Title, OwnerUsername,  ContentType, RelatedId);
            image.Id = Id;
            return image;
        }
    }
}
