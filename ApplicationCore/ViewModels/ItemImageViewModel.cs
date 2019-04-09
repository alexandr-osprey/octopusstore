using ApplicationCore.Entities;

namespace ApplicationCore.ViewModels
{
    public class ItemVariantImageViewModel: ImageViewModel<ItemVariantImage, ItemVariant>
    {
        public ItemVariantImageViewModel(): base()
        {
        }
        public ItemVariantImageViewModel(ItemVariantImage itemVariantImage): base(itemVariantImage)
        {
        }

        public override ItemVariantImage ToModel()
        {
            var image = new ItemVariantImage(Title, ContentType, RelatedId)
            {
                Id = Id
            };
            return image;
        }
    }
}
