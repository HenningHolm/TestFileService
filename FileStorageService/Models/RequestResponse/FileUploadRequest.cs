using System.IO;

namespace FileStorageService.Models.RequestResponse
{
    public class FileUploadRequest
    {
        public byte[] Content { get; set; }
        //public Stream Stream { get; set; }
        public string FileName { get; set; }

        public FileUploadRequest()
        {
        }

        public FileUploadRequest(byte[] bytes, string fileName)
        {
            FileName = fileName;
            Content = bytes;

        }

        public FileUploadRequest(Stream stream, string fileName)
        {
            AddStream(stream);
            FileName = fileName;
        }


        private void AddStream(Stream stream)
        {
            byte[] data;
            using (var br = new BinaryReader(stream))
                data = br.ReadBytes((int)stream.Length);
            Content = data;
        }
    }
}
