using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public abstract class FileInfoService<TFileInfo, TEntity>
       : Service<TFileInfo>,
        IFileInfoservice<TFileInfo, TEntity>
        where TFileInfo : FileInfo<TEntity>
        where TEntity : Entity
    {
        public FileInfoService(
            StoreContext context,
            IIdentityService identityService,
            IScopedParameters scopedParameters,
            IAuthorizationParameters<TFileInfo> authoriationParameters,
            IConfiguration configuration,
            IAppLogger<Service<TFileInfo>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            Name = typeof(TFileInfo).Name + "FileDetailService";
            _configuration = configuration;
            _rootPath = configuration["FilesFolderPath"] ?? throw new Exception("FilesFolderPath is not set in configuration");
        }

        public int MaxAllowedFileSize { get; protected set; } = 10 * 1024 * 1024;
        protected IConfiguration _configuration { get; }
        protected string _rootPath { get; }
        protected virtual IEnumerable<string> _allowedContentTypes { get; } = new List<string>();

        override public async Task<TFileInfo> CreateAsync(TFileInfo entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            string directoryPath = Path.Combine(_rootPath, GetSafeFileName(_scopedParameters.CurrentUserId));
            entity.FullPath =
                Path.Combine(directoryPath,
                    Guid.NewGuid().ToString() + GetExtension(entity.ContentType));
            await base.CreateAsync(entity);
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                await SaveFile(entity.FullPath, entity.InputStream);
            }
            catch (IOException exception)
            {
                string message = $"Error saving file {entity.FullPath} from FileInfo {nameof(TFileInfo)} Id = {entity.Id}";
                await this.DeleteSingleAsync(entity);
                _logger.Warn(exception, message);
                throw;
            }
            return entity;
        }

        public async Task<Stream> GetStreamAsync(int id)
        {
            TFileInfo entity = await _сontext.ReadByKeyAsync<TFileInfo, Service<TFileInfo>>(_logger, id);
            if (entity == null) { return null; }
            return GetStream(entity);
        }

        public Stream GetStream(TFileInfo entity)
        {
            if (entity == null) { return null; }
            try
            {
                return File.OpenRead(entity.FullPath);
            }
            catch (IOException exception)
            {
                string message = $"Error reading file {entity.FullPath} from FileInfo {nameof(TFileInfo)} Id = {entity.Id}";
                _logger.Warn(exception, message);
                throw new IOException(message, exception);
            }
        }

        override public async Task DeleteRelatedEntitiesAsync(TFileInfo entity)
        {
            try
            {
                File.Delete(entity.FullPath);
            }
            catch (IOException exception)
            {
                string message = $"Error deleting file {entity.FullPath} from FileInfo {nameof(TFileInfo)} Id = {entity.Id}";
                _logger.Warn(exception, message);
                throw new IOException(message, exception);
            }
            await base.DeleteRelatedEntitiesAsync(entity);
        }

        protected override async Task ValidateWithExceptionAsync(EntityEntry<TFileInfo> entityEntry)
        {
            await base.ValidateWithExceptionAsync(entityEntry);
            var fileInfo = entityEntry.Entity;
            if (string.IsNullOrWhiteSpace(fileInfo.Title))
                throw new EntityValidationException("Incorrect title");
            if (IsPropertyModified(entityEntry, f => f.ContentType, false) && !_allowedContentTypes.Contains(fileInfo.ContentType))
                throw new EntityValidationException($"Unsupported content type: { fileInfo.ContentType }");
            if (entityEntry.State == EntityState.Added)
                ValidateFile(fileInfo);
            else
            {
                if (IsPropertyModified(entityEntry, f => f.FullPath, false) && string.IsNullOrWhiteSpace(fileInfo.FullPath))
                    throw new EntityValidationException("Empty full path");
            }
            await ValidateRelatedEntityAsync(fileInfo);
        }

        protected async Task<bool> ValidateRelatedEntityAsync(TFileInfo fileInfo)
        {
            if (!await _сontext.ExistsBySpecAsync(_logger, new EntitySpecification<TEntity>(fileInfo.RelatedId)))
                throw new EntityValidationException($"Error saving image: related entity with Id {fileInfo.RelatedId} does not exist");
            return true;
        }

        protected bool ValidateFile(TFileInfo fileInfo)
        {
            if (fileInfo.InputStream == null || fileInfo.InputStream.Length == 0)
                throw new EntityValidationException("File not provided");
            if (fileInfo.InputStream.Length > MaxAllowedFileSize)
                throw new EntityValidationException($"The file exceeds 10 MB.");
            return true;
        }

        protected async static Task SaveFile(string fileName, Stream inputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException(nameof(inputStream));
            using (var outputStream = File.Create(fileName))
            {
                using (inputStream)
                {
                    inputStream.Seek(0, SeekOrigin.Begin);
                    await inputStream.CopyToAsync(outputStream);
                }
            }
        }


        protected string GetSafeFileName(string filename) => string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        protected string GetExtension(string contentType) => "." + contentType.Substring(contentType.IndexOf('/') + 1);
    }
}
