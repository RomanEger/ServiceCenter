using System.ComponentModel.DataAnnotations.Schema;

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
        public int ClientId { get; set;}
        public string? WhatWasDone { get; set; }

        [NotMapped]
        public string StatusStr 
            => StatusId == 1 ? "WAIT" : StatusId == 2 ? "IN_PROGRESS" : StatusId == 3 ? "DONE" : "";
        

        public Client? Client { get; set; }
        public Status? Status { get; set; } 
        public WorkType? WorkType { get; set; }
        public ICollection<WorkDetail> WorkDetails { get; set; }
        public ICollection<UserWork> UserWorks { get; set; }
    }
}
