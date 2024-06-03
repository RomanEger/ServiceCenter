using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class Work : EntityBase
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        
        public Status? Status { get; set; } 
        public ICollection<WorkDetail> WorkDetails { get; set; }
    }
}
