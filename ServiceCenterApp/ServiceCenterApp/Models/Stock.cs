using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterApp.Models
{
    public class Stock : EntityBase
    {
        public string Name { get; set; }

        public ICollection<StockDetail> StockDetails { get; set; } 
    }
}
