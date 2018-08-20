using System.IO;

namespace ApplicationCore.Entities
{
    public class ItemImage : Image<Item>
    {
        public ItemImage()
            : base()
        { }
        public ItemImage(string ownerUsername, string contentType, int relatedId, Stream inputStream)
            : base(ownerUsername, contentType, relatedId, inputStream)
        { }
        public ItemImage(string ownerUsername, string contentType, int relatedId)
            : this(ownerUsername, contentType, relatedId, null)
        { }
    }
}
