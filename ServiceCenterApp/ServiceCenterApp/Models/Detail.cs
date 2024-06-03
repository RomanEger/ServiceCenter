using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class Detail : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<StockDetail> StockDetails { get; set; }
        public ICollection<WorkDetail> WorkDetails { get; set; }
    }
}
