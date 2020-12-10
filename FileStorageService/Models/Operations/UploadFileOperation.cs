using FileStorageService.Data;
using FileStorageService.Models.Azure;
using FileStorageService.Models.RequestResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StorageService.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace FileStorageService.Models.Operations
{
    public interface IUploadFileOperation
    {
        Task<List<FileDataResponse>> ExecuteAsync(List<FileUploadRequest> files, int userId, string sourceApp, PublicAccessType accessLevel);
    }

    public class UploadFileOperation : IUploadFileOperation
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly DbContextOptions<FileStorageDbContext> _options;
        private readonly ILogger<UploadFileOperation> _logger;

        public UploadFileOperation(IAzureBlobService azureBlobService, DbContextOptions<FileStorageDbContext> options, ILogger<UploadFileOperation> logger)

        {
            _azureBlobService = azureBlobService;
            _options = options;
            _logger = logger;
        }

        public async Task<List<FileDataResponse>> ExecuteAsync(List<FileUploadRequest> files, int userId, string containerName, PublicAccessType accessType)
        {
            var result = new List<FileDataResponse>();

            foreach (var file in files)
            {
                var azureBlobServiceResponse = await _azureBlobService.UploadFile(file, userId, containerName, accessType);

                try
                {
                    var dbFile = new FileData()
                    {
                        Id = Guid.NewGuid(),
                        Filename = file.FileName.ToLower(),
                        UserId = userId,
                        FileExtension = Path.GetExtension(file.FileName)?.ToLower(),
                        FullBlobName = azureBlobServiceResponse.FullBlobName,
                        Category = azureBlobServiceResponse.FileCategory,
                        BlobUri = azureBlobServiceResponse.BlobUri,
                        SourceApplication = containerName,
                        Name = Path.GetFileNameWithoutExtension(file.FileName),
                        FileSize = file.Content.Length,
                        CreatedBy = userId.ToString(),
                        CreatedDate = DateTime.UtcNow,
                    };

                    await using var ctx = _options.Write();
                    ctx.AuditLogs.Add(new AuditLog
                    {
                        FileId = dbFile.Id,
                        Date = DateTime.UtcNow,
                        ModifiedBy = userId.ToString(),
                        UserId = userId,
                        FullBlobName = azureBlobServiceResponse.FullBlobName,
                        Action = "Upload new file"
                    });

                    ctx.Files.Add(dbFile);
                    await ctx.SaveChangesAsync();

                    result.Add(new FileDataResponse
                    {
                        Id = dbFile.Id,
                        Filename = dbFile.Filename,
                        Name = dbFile.Name,
                        FullBlobName = azureBlobServiceResponse.FullBlobName,
                        Description = dbFile.Description,
                        FileExtension = dbFile.FileExtension,
                        FileSize = dbFile.FileSize
                    });
                }
                catch (Exception e)
                {
                    _logger.LogError("UploadFile Error " + e.Message, e);
                    throw;
                }
            }
            return result;
        }

    }
}
