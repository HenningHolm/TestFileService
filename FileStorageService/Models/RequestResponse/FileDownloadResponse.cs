using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageService.Models.RequestResponse
{
    public class FileDownloadResponse
    {
        public byte[] Content { get; set; }
       // public Stream Stream { get; set; }
        public string FileName { get; set; }

        public FileDownloadResponse()
        {
        }

        public FileDownloadResponse(byte[] bytes, string fileName)
        {
            FileName = fileName;
            Content = bytes;
        }

        public FileDownloadResponse(Stream stream, string fileName)
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
