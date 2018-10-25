using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class ItemImageViewModel: ImageViewModel<ItemImage, Item>
    {
        public ItemImageViewModel()
           : base()
        {
        }
        public ItemImageViewModel(ItemImage itemImage)
           : base(itemImage)
        {
        }

        public override ItemImage ToModel()
        {
            var image = new ItemImage(Title, OwnerUsername, ContentType, RelatedId)
            {
                Id = Id
            };
            return image;
        }
    }
}
