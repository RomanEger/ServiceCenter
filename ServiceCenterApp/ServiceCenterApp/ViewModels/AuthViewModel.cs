using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ServiceCenterApp.Commands;
using ServiceCenterApp.Models;
using ServiceCenterApp.Views;
using MessageBox = System.Windows.MessageBox;

namespace ServiceCenterApp.ViewModels;

public class AuthViewModel : ViewModelBase
{
    private readonly ServiceCenterDbContext _dbContext;

    private readonly Window _authWindow;

    public AuthViewModel(ServiceCenterDbContext dbContext, Window authWindow)
    {
        User = new Employee();
        EmployeeForRegistration = new Employee();
        _dbContext = dbContext;
        LoginCommand = new MyCommand(Login);
        EmployeeRegistrationCommand = new MyCommand(EmployeeRegistration);
        _authWindow = authWindow;
        EmployeeRoles = ["ADMIN", "EMPLOYEE"];
    }
 
    public Employee User { get; set; }

    public Employee EmployeeForRegistration { get; set; }

    public string[] EmployeeRoles { get; set; }

    public string SelectedRole { get; set; }

    public ICommand LoginCommand { get; private set; }
    
    public ICommand EmployeeRegistrationCommand { get; private set; }
    
    private async void Login()
    {
        var user = await _dbContext.Employees.FirstOrDefaultAsync(u => u.Login == User.Login && u.Password == User.Password);
        if (user is null)
            MessageBox.Show("Ошибка при входе");
        else
        {
            UserRole.Role = user.RoleId switch
            {
                1 => RoleName.ADMIN,
                2 => RoleName.EMPLOYEE,
                _ => null
            };
            MainWindow.Employee = user;
            _authWindow.Close();
        }
    }

    private async void EmployeeRegistration()
    {
        var user = await _dbContext.Employees.FirstOrDefaultAsync(u => u.Login == EmployeeForRegistration.Login);
        if (user is not null)
        {
            MessageBox.Show("Логин занят");
        }
        else
        {
            if(SelectedRole == EmployeeRoles[0])
            {
                EmployeeForRegistration.RoleId = 1;
            }
            else if(SelectedRole == EmployeeRoles[1])
            {
                EmployeeForRegistration.RoleId = 2;
            }
            await _dbContext.Employees.AddAsync(EmployeeForRegistration);
            await _dbContext.SaveChangesAsync();
            _authWindow.Close();
        }
    }
}