using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Data
{
    public interface IHaveTimestamps
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
