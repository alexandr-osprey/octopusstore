using System.IO;

namespace ApplicationCore.Entities
{
    public abstract class Image<T> : FileInfo<T> where T : Entity
    {
        public Image()
            : base()
        { }
        public Image(string ownerId,string contentType, int relatedId, Stream inputStream)
            : base(ownerId, contentType, relatedId, inputStream)
        { }
        public Image(string ownerId, string contentType, int relatedId)
            : this(ownerId,  contentType, relatedId, null)
        { }
    }
}
