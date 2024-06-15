namespace ServiceCenterApp.Models
{
    public class Status : EntityBase
    {
        public StatusName StatusName { get; set; }
        public ICollection<Work> Works { get; set; }
    }

    public enum StatusName
    {
        WAIT,
        IN_PROGRESS,
        DONE
    }
}
