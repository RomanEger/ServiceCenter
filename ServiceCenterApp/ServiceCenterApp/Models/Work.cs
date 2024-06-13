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
        public int WorkTypeId { get; set; }
        public string? Description { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Status? Status { get; set; } 
        public WorkType? WorkType { get; set; }
        public ICollection<WorkDetail> WorkDetails { get; set; }
        public ICollection<UserWork> UserWorks { get; set; }
    }
}
