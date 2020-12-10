using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using FileStorageService.Models.RequestResponse;

namespace FileStorageService.Models.Azure
{
    public interface IAzureBlobService
    {
        Task<FileUploadResponse> UploadFile(FileUploadRequest files, int? personId, string containerName, PublicAccessType accessLevel);
        Task<Stream> DownloadFile(string fullBlobName, string containerName);
        string CreateSharedAccess(string blobName, string containerName);
    }

    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AzureBlobService> _logger;
        private BlobContainerClient _containerClient;
        private static BlobServiceClient _blobServiceClient;


        public AzureBlobService(IConfiguration configuration, ILogger<AzureBlobService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<FileUploadResponse> UploadFile(FileUploadRequest file, int? personId, string containerName, PublicAccessType accessLevel)
        {
       
            var container = await GetBlobContainer(containerName.ToLower(), accessLevel);

            var blobName = file.FileName;

            var blobClient = container.GetBlobClient(blobName);

            await using var stream = new MemoryStream(file.Content);
            await blobClient.UploadAsync(stream);

            var azureBlobServiceResponse = new FileUploadResponse()
            {
                FullBlobName = blobName,
                BlobUri = blobClient.Uri.ToString(),
                FileCategory = containerName
            };
            return azureBlobServiceResponse;
        }

        public async Task<Stream> DownloadFile(string fullBlobName, string containerName)
        {

            var container = containerName.ToLower();
            var blobContainerClient = await GetBlobContainer(container);
            var blobClient = blobContainerClient.GetBlobClient(fullBlobName);
            
            var blob = blobClient.Download();
            var content = blob.Value.Content;

            return content;

        }

        public string CreateSharedAccess(string blobName, string containerName)
        {

            var container = containerName.ToLower();
            var accountName = _configuration.GetValue<string>("StorageAccount:AccountName");
            var accountKey = _configuration.GetValue<string>("StorageAccount:AccountKey");

            BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container,
                BlobName = blobName,
                Resource = "b", //b = blob, c = container
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(30),
            };

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            StorageSharedKeyCredential storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
          
            // Use the key to get the SAS token.
            var sasTolken = '?'+ blobSasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();
            return sasTolken;

        }


        /// <summary> 
        /// Gets Azure BlobStorage Container Client (SourceApp level)
        /// </summary> 
        private async Task<BlobContainerClient> GetBlobContainer(string containerName, PublicAccessType accessLevel = 0)
        {
            _blobServiceClient = GetServiceClient();
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            if (!_containerClient.Exists())
            {
                _containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName, accessLevel);
            }
            return _containerClient;
        }

        /// <summary> 
        /// Gets Azure BlobStorage Account Client (Top level)
        /// </summary> 
        private BlobServiceClient GetServiceClient()
        {
            if (_blobServiceClient != null)
                return _blobServiceClient;

            var storageConnectionString = _configuration.GetConnectionString("AzureConnectionString");
            if (string.IsNullOrWhiteSpace(storageConnectionString))
            {
                throw new ArgumentException("Configuration must contain AzureConnectionString");
            }

            try
            {
                _blobServiceClient = new BlobServiceClient(storageConnectionString);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not create storage account with AzureConnectionString configuration");
                throw;
            }

            return _blobServiceClient;
        }


        /// <summary> 
        /// Generates a unique random fileId for blobname in Azure 
        /// </summary> 
        private string GetRandomBlobId(string filename)
        {
            return string.Format("{0:10}_{1}", DateTime.Now.Ticks, Guid.NewGuid());
        }

    }

  }
