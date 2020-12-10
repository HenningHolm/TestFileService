using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileStorageService.Models.Azure;
using FileStorageService.Models.Queries;
using FileStorageService.Models.RequestResponse;
using Microsoft.Extensions.Logging;

namespace FileStorageService.Models.Operations
{
    public interface IGenerateSharedAccessUriOperation
    {
        Task<string> ExecuteAsync(Guid fileId);
    }

    public class GenerateSharedAccessUriOperation : IGenerateSharedAccessUriOperation
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly IGetFileDataByIdQuery _getFileDataByIdQuery;
        private readonly ILogger<DownloadFileOperation> _logger;

        public GenerateSharedAccessUriOperation(IAzureBlobService azureBlobService, IGetFileDataByIdQuery getFileDataByIdQuery, ILogger<DownloadFileOperation> logger)
        {
            _getFileDataByIdQuery = getFileDataByIdQuery;
            _azureBlobService = azureBlobService;
            _logger = logger;
        }

        public async Task<string> ExecuteAsync(Guid fileId)
        {

            var blobData = await _getFileDataByIdQuery.ExecuteAsync(fileId);
            if (blobData == null)
            {
                return null;
            }

            var sasUri = _azureBlobService.CreateSharedAccess(blobData.FullBlobName, blobData.SourceApp);
            return blobData.BlobUri+sasUri ;

        }
    }
}
