using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class StockDetail : EntityBase
    {
        public int StockId { get; set; }
        public int DetailId { get; set; }
        public int CountDetail { get; set; }

        public Stock? Stock { get; set; }
        public Detail? Detail { get; set; }
    }
}
