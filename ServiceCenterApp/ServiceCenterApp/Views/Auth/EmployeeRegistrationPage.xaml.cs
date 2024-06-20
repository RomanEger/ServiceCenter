using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Auth
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class EmployeeRegistrationPage : Page
    {
        public EmployeeRegistrationPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var page = new LoginPage();
            page.DataContext = DataContext;
            Navigation.Frame?.Navigate(page);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Navigation.Frame.GoBack();
        }
    }
}
