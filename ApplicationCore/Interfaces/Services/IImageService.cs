using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.Services
{
    /// <summary>
    /// Maintains full lifecycle of Image entities
    /// </summary>
    /// <typeparam name="TFileInfo"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IImageService<TFileInfo, TEntity> 
       : IFileInfoservice<TFileInfo, TEntity> where TFileInfo: Image<TEntity> where TEntity: Entity
    {
    }
}
