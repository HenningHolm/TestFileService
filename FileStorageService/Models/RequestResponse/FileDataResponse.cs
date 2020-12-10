using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageService.Models.RequestResponse
{
    public class FileDataResponse
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public string FullBlobName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string Description { get; set; }

    }
}
