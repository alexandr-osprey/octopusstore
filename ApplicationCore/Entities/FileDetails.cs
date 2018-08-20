using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ApplicationCore.Entities
{
    public abstract class FileDetails<T> : Entity
    {
        public static readonly string rootDirectory = DefaultSettings.RootFileDirectory;

        public string Title { get; set; }
        public string ContentType { get; set; }
        public int RelatedId { get; set; }
        public string FullPath { get; set; }
        public string DirectoryPath { get; set; }
        public string OwnerUsername { get; set; }

        [NotMapped]
        public Stream InputStream { get; set; }
        public T RelatedEntity { get; set; }

        public FileDetails()
        {  }
        public FileDetails(string ownerUsername, string contentType, int relatedId, Stream inputStream)
        {
            ContentType = contentType;
            RelatedId = relatedId;
            OwnerUsername = ownerUsername;
            InputStream = inputStream;
            DirectoryPath =
                Path.Combine(rootDirectory, GetSafeFileName(ownerUsername));
            FullPath = 
                Path.Combine(DirectoryPath, 
                    Guid.NewGuid().ToString() + GetExtension(contentType));
        }

        public static string GetSafeFileName(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
        public static string GetExtension(string contentType)
        {
            return "." + contentType.Substring(contentType.IndexOf('/') + 1);
        }
    }
}
