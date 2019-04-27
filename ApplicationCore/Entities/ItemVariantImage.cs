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

        public ItemVariantImage(string title, string contentType, int relatedId, string fullPath, Stream inputStream)
           : base(title, contentType, relatedId, null, inputStream)
        {
        }

        protected ItemVariantImage(ItemVariantImage itemVariantImage): base(itemVariantImage)
        {
        }

        public ItemVariantImage ShallowClone()
        {
            return new ItemVariantImage();
        }
    }
}
