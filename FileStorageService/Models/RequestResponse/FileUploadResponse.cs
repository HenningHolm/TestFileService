namespace FileStorageService.Models.RequestResponse
{
    public class FileUploadResponse
    {
        public string FullBlobName { get; set; }
        public string FileCategory { get; set; }
        public string BlobUri { get; set; }

    }
}
