namespace ServiceCenterApp.Models
{
    public class UserWork : EntityBase
    {
        public int WorkId { get; set; }
        public int EmployeeId { get; set; }

        public Work? Work { get; set; }
        public Employee? Employee { get; set; }
    }
}
