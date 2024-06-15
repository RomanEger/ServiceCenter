namespace ServiceCenterApp.Models
{
    public class Stock : EntityBase
    {
        public string Name { get; set; }

        public ICollection<StockDetail> StockDetails { get; set; } 
    }
}
