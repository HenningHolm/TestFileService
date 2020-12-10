using System;
using System.Threading.Tasks;
using FileStorageService.Data;
using Microsoft.EntityFrameworkCore;
using StorageService.Data;

namespace FileStorageService.Models.Queries
{
    public interface IGetFileDataByIdQuery
    {
        Task<DownloadFileData> ExecuteAsync(Guid fileId);
    }

    public class GetFileDataByIdQuery : IGetFileDataByIdQuery
    {
        private readonly DbContextOptions<FileStorageDbContext> _dbContextOptions;

        public GetFileDataByIdQuery(DbContextOptions<FileStorageDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }
        public async Task<DownloadFileData> ExecuteAsync(Guid fileId)
        {
            await using var ctx = _dbContextOptions.Read();
            var file = await ctx.Files.FirstOrDefaultAsync(o => o.Id == fileId);

            if (file == null)
            {
                return null;
            }

            return new DownloadFileData()
            {
                FullBlobName = file.FullBlobName,
                FileExtension = file.FileExtension,
                SourceApp = file.SourceApplication,
                FileName = file.Filename,
                BlobUri = file.BlobUri
            };
        }
    }

    public class DownloadFileData
    {
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string SourceApp { get; set; }
        public string FullBlobName { get; set; }
        public string BlobUri { get; set; }
    }


}
