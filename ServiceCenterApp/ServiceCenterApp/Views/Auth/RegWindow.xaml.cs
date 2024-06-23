using System.Windows;
using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;

namespace ServiceCenterApp.Views.Auth;

public partial class RegWindow : Window
{
    public RegWindow(ServiceCenterDbContext dbContext)
    {
        InitializeComponent();
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        WindowState = WindowState.Normal;
        DataContext = new AuthViewModel(dbContext, this);
    }
}