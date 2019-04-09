using ApplicationCore.Interfaces;
using System.IO;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image of an Item Variant
    /// </summary>
    public class ItemVariantImage: Image<ItemVariant>, ShallowClonable<ItemVariantImage>
    {
        public ItemVariantImage(): base()
        {
        }

        public ItemVariantImage(string title, string contentType, int relatedId, Stream inputStream)
           : base(title, contentType, relatedId, inputStream)
        {
        }

        public ItemVariantImage(string title, string contentType, int relatedId)
           : this(title, contentType, relatedId, null)
        {
        }

        protected ItemVariantImage(ItemVariantImage itemImage): base(itemImage)
        {
        }

        public ItemVariantImage ShallowClone()
        {
            return (ItemVariantImage)MemberwiseClone();
        }
    }
}
