using ApplicationCore.Entities;

namespace ApplicationCore.Specifications
{
    public abstract class ImageDetailSpecification<TFileDetails, TEntity> : DetailSpecification<TFileDetails> where TFileDetails: Image<TEntity> where TEntity : Entity
    {
        public ImageDetailSpecification(int id)
            : base(id)
        {  }
    }
}
