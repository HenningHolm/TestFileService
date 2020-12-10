using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Data
{
    public class FileData : IHaveTimestamps
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string Description { get; set; }
        public string FullBlobName { get; set; }
        public string Category { get; set; }
        public string SourceApplication { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string BlobUri { get; set; }
    }
}
