using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views.Auth;
using System.Windows;

namespace ServiceCenterApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AutWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow(ServiceCenterDbContext dbContext)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            WindowState = WindowState.Normal;
            DataContext = new AuthViewModel(dbContext, this);
        }
    }
}
