using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.ReportViews
{
    public class WorkView
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get;set; }
        public DateTime EndDate { get;set; }
        public string TypeName { get; set; }
        public decimal TotalCost { get; set; }
    }
}
