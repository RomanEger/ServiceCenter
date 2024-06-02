using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServiceCenterApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AutWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly AuthViewModel _viewModel;

        private readonly LoginPage _loginPage;

        private readonly ServiceCenterDbContext _dbContext;
        public AuthWindow(ServiceCenterDbContext dbContext)
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            WindowState = WindowState.Normal;
            _loginPage = new LoginPage();
            _dbContext = dbContext;
            _loginPage.DataContext = _viewModel = new AuthViewModel(_dbContext, this);
            Navigation.Frame = AuthFrame;
            Navigation.Frame?.Navigate(_loginPage);
        }
    }
}
