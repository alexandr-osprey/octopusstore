using System.IO;

namespace ApplicationCore.Entities
{
    public abstract class Image<T> : FileDetails<T> where T : Entity
    {
        public Image()
            : base()
        { }
        public Image(string ownerUsername,string contentType, int relatedId, Stream inputStream)
            : base(ownerUsername, contentType, relatedId, inputStream)
        { }
        public Image(string ownerUsername, string contentType, int relatedId)
            : this(ownerUsername,  contentType, relatedId, null)
        { }
    }
}
