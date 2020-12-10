using FileStorageService.Data;
using FileStorageService.Models.Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StorageService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageService.Models.Operations
{
    public interface IDeleteFileOperation
    {
        Task<Guid?> ExecuteAsync(Guid fileGuid, int userId);
    }

    public class DeleteFileOperation : IDeleteFileOperation
    {
        private readonly DbContextOptions<FileStorageDbContext> _dbContextOptions;
        private readonly ILogger<DeleteFileOperation> _logger;

        public DeleteFileOperation(IAzureBlobService azureBlobService, DbContextOptions<FileStorageDbContext> dbContextOptions, ILogger<DeleteFileOperation> logger)
        {
            _dbContextOptions = dbContextOptions;
            _logger = logger;
        }
        public async Task<Guid?> ExecuteAsync(Guid fileGuid, int userId)
        {
            await using (var ctx = _dbContextOptions.Write())
            {
                var file = ctx.Files.SingleOrDefault(f => f.Id == fileGuid);
                if (file != null)
                {
                    file.ModifiedBy = userId.ToString();
                    file.ModifiedDate = DateTime.UtcNow;
                    ctx.Files.Remove(file);

                    ctx.AuditLogs.Add(new AuditLog
                    {
                        FileId = fileGuid,
                        Date = DateTime.UtcNow,
                        FullBlobName = file.FullBlobName, 
                        UserId = file.UserId,
                        ModifiedBy = userId.ToString(),
                        Action = "Delete file"
                    });
                    await ctx.SaveChangesAsync();

                    return fileGuid;

                }
            }
            return null;
        }
    }
}
