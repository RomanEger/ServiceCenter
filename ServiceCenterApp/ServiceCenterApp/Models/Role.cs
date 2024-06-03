namespace ServiceCenterApp.Models;

public class Role : EntityBase
{
    public RoleName RoleName { get; set; }
    
    public ICollection<Employee> Users { get; set; }
}

public enum RoleName
{
    ADMIN,
    EMPLOYEE
}