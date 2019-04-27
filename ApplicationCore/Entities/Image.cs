using System.Collections.Generic;
using System.IO;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Info about stored Images
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Image<T>: FileInfo<T> where T: Entity
    {
        public Image(): base()
        {
        }
        public Image(string title, string contentType, int relatedId, string fullPath, Stream inputStream): base(title, contentType, relatedId, fullPath, inputStream)
        {
        }
        public Image(string title, string contentType, int relatedId, string fullPath) : this(title,  contentType, relatedId, fullPath, null)
        {
        }
        protected Image(Image<T> image): base(image)
        {
        }
    }
}
