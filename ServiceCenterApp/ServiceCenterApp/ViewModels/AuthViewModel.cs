using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using ServiceCenterApp.Views;

namespace ServiceCenterApp.ViewModels;

public class AuthViewModel : ViewModelBase
{
    private readonly ServiceCenterDbContext _dbContext;

    private readonly AuthWindow _authWindow;

    public AuthViewModel(ServiceCenterDbContext dbContext, AuthWindow authWindow)
    {
        User = new User();
        UserForRegistration = new User();
        _dbContext = dbContext;
        LoginCommand = new MyCommand(Login);
        RegistrationCommand = new MyCommand(Registration);
        _authWindow = authWindow;
        Roles = new List<string>()
        {
            "ADMIN",
            "EMPLOYEE",
            "CLIENT"
        };
    }
 
    public User User { get; set; }

    public User UserForRegistration { get; set; }

    public IEnumerable<string> Roles { get; set; }

    public void Notify(string info)
    {
        MessageBox.Show(info);
    }

    public ICommand LoginCommand { get; private set; }
    
    public ICommand RegistrationCommand { get; private set; }
    
    private async void Login()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == User.Login && u.Password == User.Password);
        if (user is null)
            Notify("Ошибка при входе");
        else
        {
            UserRole.Role = user.RoleId == 1 ?
                RoleName.ADMIN :
                user.RoleId == 2 ?
                RoleName.EMPLOYEE : null;
            _authWindow.Close();
        }
    }

    private async void Registration()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == UserForRegistration.Login);
        if (user is not null)
        {
            Notify("Логин занят");
        }
        else
        {
            //клиент
            UserForRegistration.RoleId = 3;
            UserRole.Role = RoleName.CLIENT;
            await _dbContext.Users.AddAsync(UserForRegistration);
            await _dbContext.SaveChangesAsync();
            _authWindow.Close();
        }
    }
}