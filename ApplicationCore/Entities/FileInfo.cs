using ApplicationCore.Extensions;
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
        public string Title { get; set; } = string.Empty;
        public string ContentType { get; set; }
        public int RelatedId { get; set; }
        public string FullPath { get; set; } = string.Empty;

        [NotMapped]
        public Stream InputStream { get; set; }
        public virtual T RelatedEntity { get; set; }

        public FileInfo()
        {
        }

        public bool Equals(FileInfo<T> other) => base.Equals(other)
            && Title.EqualsCI(other.Title)
            && ContentType == other.ContentType
            && RelatedId == other.RelatedId
            && FullPath.EqualsCI(other.FullPath);
        public override bool Equals(object obj) => Equals(obj as FileInfo<T>);
        public override int GetHashCode() => base.GetHashCode();

        public FileInfo(string title, string contentType, int relatedId, string fullPath, Stream inputStream)
        {
            Title = title;
            ContentType = contentType;
            RelatedId = relatedId;
            InputStream = inputStream;
            FullPath = fullPath;
        }

        protected FileInfo(FileInfo<T> fileInfo): base(fileInfo)
        {
            Title = fileInfo.Title;
            ContentType = fileInfo.ContentType;
            RelatedId = fileInfo.RelatedId;
            FullPath = fileInfo.FullPath;
            InputStream = fileInfo.InputStream;
        }
    }
}
