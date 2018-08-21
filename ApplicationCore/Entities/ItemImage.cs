using System.IO;

namespace ApplicationCore.Entities
{
    public class ItemImage : Image<Item>
    {
        public ItemImage()
            : base()
        { }
        public ItemImage(string ownerId, string contentType, int relatedId, Stream inputStream)
            : base(ownerId, contentType, relatedId, inputStream)
        { }
        public ItemImage(string ownerId, string contentType, int relatedId)
            : this(ownerId, contentType, relatedId, null)
        { }
    }
}
