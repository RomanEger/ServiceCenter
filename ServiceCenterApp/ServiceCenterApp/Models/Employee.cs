namespace ServiceCenterApp.Models;

public class Employee : EntityBase
{
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public int RoleId { get; set; }
    
    public Role? Role { get; set; }
    public ICollection<UserWork> UserWorks { get; set; }
}