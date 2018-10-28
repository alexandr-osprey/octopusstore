using System.Collections.Generic;
using System.IO;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Image of an Item
    /// </summary>
    public class ItemImage: Image<Item>
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
    }
}
