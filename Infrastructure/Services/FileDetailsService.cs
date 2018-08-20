using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public abstract class Filedetailservice<TFileDetails, TEntity> 
        : Service<TFileDetails>, 
        IFiledetailservice<TFileDetails, TEntity> 
        where TFileDetails : FileInfo<TEntity> 
        where TEntity : Entity
    {
        public Filedetailservice(
            IAsyncRepository<TFileDetails> repository,
            IAppLogger<Service<TFileDetails>> logger)
            : base(repository, logger)
        {
            Name = typeof(TFileDetails).Name + "Filedetailservice";
        }

        override public async Task<TFileDetails> AddAsync(TFileDetails entity)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            await base.AddAsync(entity);
            try
            {
                if (!Directory.Exists(entity.DirectoryPath)) { Directory.CreateDirectory(entity.DirectoryPath); }
                await SaveFile(entity.FullPath, entity.InputStream);
            }
            catch (IOException exception)
            {
                string message = $"Error saving file {entity.FullPath} from FileDetails {nameof(TFileDetails)} Id = {entity.Id}";
                _logger.Warn(exception, message);
                throw new IOException(message, exception);
            }
            return entity;
        }
        public async Task<Stream> GetStreamAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) { return null; }
            return GetStream(entity);
        }
        public Stream GetStream(TFileDetails entity)
        {
            if (entity == null) { return null; }
            try
            {
                return File.OpenRead(entity.FullPath);
            }
            catch (IOException exception)
            {
                string message = $"Error reading file {entity.FullPath} from FileDetails {nameof(TFileDetails)} Id = {entity.Id}";
                _logger.Warn(exception, message);
                throw new IOException(message, exception);
            }
        }
        override public async Task DeleteRelatedEntitiesAsync(TFileDetails entity)
        {
            try
            {
                File.Delete(entity.FullPath);
            }
            catch (IOException exception)
            {
                string message = $"Error deleting file {entity.FullPath} from FileDetails {nameof(TFileDetails)} Id = {entity.Id}";
                _logger.Warn(exception, message);
                throw new IOException(message, exception);
            }
            await base.DeleteRelatedEntitiesAsync(entity);
        }

        private async static Task SaveFile(string fileName, Stream inputStream)
        {
            if (inputStream == null) { throw new ArgumentNullException(nameof(inputStream)); }
            using (var outputStream = File.Create(fileName))
            {
                using (inputStream)
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                    await inputStream.CopyToAsync(outputStream);
                }
            }
        }
    }
}
