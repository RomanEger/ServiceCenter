using ServiceCenterApp.Models;
using System.Windows;
using System.Windows.Input;

namespace ServiceCenterApp.Views;

public partial class MainWindow : Window
{
    private ServiceCenterDbContext _dbContext { get;}

    public MainWindow(ServiceCenterDbContext dbContext, AuthWindow authWindow)
    {
        InitializeComponent();
        WindowState = WindowState.Maximized;
        _dbContext = dbContext;
        authWindow.ShowDialog();
        if (UserRole.Role == null)
        {
            //все норм
            throw new Exception();
        }
        //добавить авторизацию
        Navigation.Frame = MainFrame;
    }
}