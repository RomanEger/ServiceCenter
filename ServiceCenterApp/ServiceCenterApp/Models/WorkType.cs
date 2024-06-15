namespace ServiceCenterApp.Models
{
    public class WorkType : EntityBase
    {
        public string Type { get; set; }
        public ICollection<Work> Works { get; set; }
    }
}
