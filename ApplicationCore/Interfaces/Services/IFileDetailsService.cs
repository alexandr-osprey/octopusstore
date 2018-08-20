using ApplicationCore.Entities;
using System.IO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IFileDetailsService<TFileDetails, TEntity> : IService<TFileDetails> where TFileDetails : FileDetails<TEntity> where TEntity: Entity
    {
        Task<Stream> GetStreamAsync(int id);
        Stream GetStream(TFileDetails entity);
    }
}
