using ApplicationCore.Interfaces;
using System.IO;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image of an Item
    /// </summary>
    public class ItemImage: Image<Item>, ShallowClonable<ItemImage>
    {
        public ItemImage(): base()
        {
        }

        public ItemImage(string title, string contentType, int relatedId, Stream inputStream)
           : base(title, contentType, relatedId, inputStream)
        {
        }

        public ItemImage(string title, string contentType, int relatedId)
           : this(title, contentType, relatedId, null)
        {
        }

        protected ItemImage(ItemImage itemImage): base(itemImage)
        {
        }

        public ItemImage ShallowClone()
        {
            return (ItemImage)MemberwiseClone();
        }
    }
}
