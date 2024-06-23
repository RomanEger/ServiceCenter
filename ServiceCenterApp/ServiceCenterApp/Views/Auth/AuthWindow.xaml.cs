using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using System.Windows;

namespace ServiceCenterApp.Views
{
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
