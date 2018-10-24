using System.Collections.Generic;
using System.IO;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image of an Item
    /// </summary>
    public class ItemImage : Image<Item>
    {
        public ItemImage(): base()
        {
        }

        public ItemImage(string title, string ownerId, string contentType, int relatedId, Stream inputStream)
            : base(title, ownerId, contentType, relatedId, inputStream)
        {
        }
        public ItemImage(string title, string ownerId, string contentType, int relatedId)
            : this(title, ownerId, contentType, relatedId, null)
        {
        }
    }
}
