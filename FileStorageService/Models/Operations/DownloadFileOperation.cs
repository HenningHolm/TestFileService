using System;
using System.IO;
using System.Threading.Tasks;
using FileStorageService.Models.Azure;
using FileStorageService.Models.RequestResponse;
using Microsoft.Extensions.Logging;

namespace FileStorageService.Models.Queries
{
    public interface IDownloadFileOperation
    {
        Task<FileDownloadResponse> ExecuteAsync(Guid fileId);
    }

    public class DownloadFileOperation : IDownloadFileOperation
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly IGetFileDataByIdQuery _getFileDataByIdQuery;
        private readonly ILogger<DownloadFileOperation> _logger;

        public DownloadFileOperation(IAzureBlobService azureBlobService, IGetFileDataByIdQuery getFileDataByIdQuery, ILogger<DownloadFileOperation> logger)
        {
            _getFileDataByIdQuery = getFileDataByIdQuery;
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        public async Task<FileDownloadResponse> ExecuteAsync(Guid fileId)
        {

            var azureDownloadData = await _getFileDataByIdQuery.ExecuteAsync(fileId);
            if (azureDownloadData == null)
            {
                return null;
            }

            var stream = await _azureBlobService.DownloadFile(azureDownloadData.FullBlobName, azureDownloadData.SourceApp);
            if (stream == null)
            {
                return null;
            }

            await using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            var response = new FileDownloadResponse(memoryStream, azureDownloadData.FileName);
            return response;

        }
    }
}
