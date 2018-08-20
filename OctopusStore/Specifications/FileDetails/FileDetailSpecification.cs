using ApplicationCore.Entities;

namespace OctopusStore.Specifications
{
    public abstract class FileDetailSpecification<TFileDetails, TEntity> 
        : DetailSpecification<TFileDetails> where TFileDetails: FileInfo<TEntity> where TEntity : Entity
    {
        public FileDetailSpecification(int id)
            : base(id)
        {  }
    }
}
