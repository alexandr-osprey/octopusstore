using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Maintains full lifecycle of Image entities
    /// </summary>
    /// <typeparam name="TFileDetails"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IImageService<TFileDetails, TEntity> 
       : IFiledetailservice<TFileDetails, TEntity> where TFileDetails: Image<TEntity> where TEntity: Entity
    {
    }
}
