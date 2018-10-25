using ApplicationCore.Entities;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    ///  Maintains full lifecycle of FileInfo entities
    /// </summary>
    /// <typeparam name="TFileDetails"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IFiledetailservice<TFileDetails, TEntity>: IService<TFileDetails> where TFileDetails: FileInfo<TEntity> where TEntity: Entity
    {
        /// <summary>
        /// Max allowed file zise
        /// </summary>
        int MaxAllowedFileSize { get; set; }
        /// <summary>
        /// Retrieves file stream based on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Stream> GetStreamAsync(int id);
        /// <summary>
        /// Retrieves file stream of FileInfo
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Stream GetStream(TFileDetails entity);
    }
}
