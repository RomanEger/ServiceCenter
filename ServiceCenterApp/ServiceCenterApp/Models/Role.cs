namespace ServiceCenterApp.Models;

public class Role : EntityBase
{
    public RoleName RoleName { get; set; }
    
    public ICollection<User> Users { get; set; }
}

public enum RoleName
{
    ADMIN,
    EMPLOYEE,
    CLIENT
}