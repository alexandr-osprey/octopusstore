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
        public override IEnumerable<string> AllowedContentTypes { get; } = new List<string>() { @"image/jpeg", @"image/jpg", @"image/png" };

        public Image(): base()
        {
        }
        public Image(string title, string contentType, int relatedId, Stream inputStream): base(title, contentType, relatedId, inputStream)
        {
        }
        public Image(string title, string contentType, int relatedId): this(title,  contentType, relatedId, null)
        {
        }
        protected Image(Image<T> image): base(image)
        {
        }
    }
}
