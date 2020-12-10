using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorageService.Data
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public Guid FileId { get; set; }
        public int UserId { get; set; }
        public string ModifiedBy { get; set; }
        public string FullBlobName { get; set; }
        public DateTime Date { get; set; }
    }
}
