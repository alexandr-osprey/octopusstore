using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface IImageService<TFileDetails, TEntity> 
        : IFiledetailservice<TFileDetails, TEntity> where TFileDetails : Image<TEntity> where TEntity : Entity
    {  }
}
