using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class WorkType : EntityBase
    {
        public string Type { get; set; }
        public ICollection<Work> Works { get; set; }
    }
}
