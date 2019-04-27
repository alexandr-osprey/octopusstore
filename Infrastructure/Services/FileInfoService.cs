﻿using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Identity;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Services;
using ApplicationCore.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.IO;
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
            IAppLogger<Service<TFileInfo>> logger)
           : base(context, identityService, scopedParameters, authoriationParameters, logger)
        {
            Name = typeof(TFileInfo).Name + "Filedetailservice";
        }

        public int MaxAllowedFileSize { get; protected set; } = 10 * 1024 * 1024;

        override public async Task<TFileInfo> CreateAsync(TFileInfo entity)
        {
            await base.CreateAsync(entity);
            try
            {
                if (!Directory.Exists(entity.DirectoryPath))
                    Directory.CreateDirectory(entity.DirectoryPath);
                await SaveFile(entity.FullPath, entity.InputStream);
            }
            catch (IOException exception)
            {
                string message = $"Error saving file {entity.FullPath} from FileInfo {nameof(TFileInfo)} Id = {entity.Id}";
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
            if (IsPropertyModified(entityEntry, f => f.ContentType, false) && !fileInfo.ContentTypeAllowed)
                throw new EntityValidationException($"Unsupported content type: { fileInfo.ContentType }");
            if (entityEntry.State == EntityState.Added)
                ValidateFile(fileInfo);
            else
            {
                if (IsPropertyModified(entityEntry, f => f.FullPath, false) && string.IsNullOrWhiteSpace(fileInfo.FullPath))
                    throw new EntityValidationException("Empty full path");
                if (IsPropertyModified(entityEntry, f => f.DirectoryPath, false) && string.IsNullOrWhiteSpace(fileInfo.DirectoryPath))
                    throw new EntityValidationException("Empty directory path");
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
    }
}
