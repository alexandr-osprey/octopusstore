using ApplicationCore.Entities;

namespace OctopusStore.ViewModels
{
    public class ItemImageViewModel : ImageViewModel<ItemImage, Item>
    {
        public ItemImageViewModel()
            : base()
        { }
        public ItemImageViewModel(ItemImage itemImage)
            : base(itemImage)
        { }

        public override ItemImage ToModel()
        {
            var image = new ItemImage(OwnerUsername,  ContentType, RelatedId);
            image.Title = Title;
            image.Id = Id;
            return image;
        }
    }
}
