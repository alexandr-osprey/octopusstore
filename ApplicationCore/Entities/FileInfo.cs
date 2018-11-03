using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Info about any stored file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FileInfo<T>: Entity
    {
        public static readonly string rootDirectory = DefaultSettings.RootFileDirectory;

        public virtual IEnumerable<string> AllowedContentTypes { get; } = new List<string>();

        public string Title { get; set; } = string.Empty;
        public string ContentType { get; set; }
        public int RelatedId { get; set; }
        public string FullPath { get; set; } = string.Empty;
        public string DirectoryPath { get; set; } = string.Empty;

        [NotMapped]
        public Stream InputStream { get; set; }
        public T RelatedEntity { get; set; }

        public FileInfo()
        {
        }
        public FileInfo(string title, string contentType, int relatedId, Stream inputStream)
        {
            Title = title;
            ContentType = contentType;
            RelatedId = relatedId;
            InputStream = inputStream;
        }
        protected FileInfo(FileInfo<T> fileInfo): base(fileInfo)
        {
            Title = fileInfo.Title;
            ContentType = fileInfo.ContentType;
            RelatedId = fileInfo.RelatedId;
            FullPath = fileInfo.FullPath;
            DirectoryPath = fileInfo.DirectoryPath;
        }

        public override string OwnerId
        {
            get => base.OwnerId;
            set
            {
                base.OwnerId = value;
                DirectoryPath =
                Path.Combine(rootDirectory, GetSafeFileName(OwnerId));
                FullPath =
                    Path.Combine(DirectoryPath,
                        Guid.NewGuid().ToString() + GetExtension(ContentType));
            }
        }

        public bool ContentTypeAllowed => AllowedContentTypes.Contains(ContentType);

        public static string GetSafeFileName(string filename) => string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        public static string GetExtension(string contentType) => "." + contentType.Substring(contentType.IndexOf('/') + 1);
    }
}
