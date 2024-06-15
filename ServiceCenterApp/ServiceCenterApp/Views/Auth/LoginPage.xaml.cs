using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Auth
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new EmployeeRegistrationPage();
            page.DataContext = DataContext;
            Navigation.Frame?.Navigate(page);
        }
    }
}
