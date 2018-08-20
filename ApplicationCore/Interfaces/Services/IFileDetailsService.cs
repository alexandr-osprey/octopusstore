using ApplicationCore.Entities;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IFiledetailservice<TFileDetails, TEntity> : IService<TFileDetails> where TFileDetails : FileInfo<TEntity> where TEntity: Entity
    {
        Task<Stream> GetStreamAsync(int id);
        Stream GetStream(TFileDetails entity);
    }
}
