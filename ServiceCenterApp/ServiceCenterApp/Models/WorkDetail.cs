using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class WorkDetail : EntityBase
    {
        public int WorkId { get; set; }
        public int DetailId { get; set; }
        public int Count { get; set; }

        public Work? Work { get; set; }
        public Detail? Detail { get; set; }
    }
}
