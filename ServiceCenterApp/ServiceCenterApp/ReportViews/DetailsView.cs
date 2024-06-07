using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.ReportViews
{
    public class DetailsView
    {
        public string WorkName { get; set; }
        public string DetailName {  get; set; }
        public decimal DetailPrice { get; set; }
        public int DetailCount { get; set; }
        public decimal TotalCost { get; set; }
    }
}
