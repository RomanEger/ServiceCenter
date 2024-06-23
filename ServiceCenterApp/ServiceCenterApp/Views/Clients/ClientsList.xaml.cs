using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Clients
{
    public partial class ClientsList : Page
    {
        public ClientsList()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var w = new ClientRegistrationWindow();
            w.DataContext = this.DataContext;
            w.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (UserRole.Role == RoleName.EMPLOYEE)
            {
                MessageBox.Show("Удалить может только админ");
                return;
            }
            var dataContext = (ClientViewModel)DataContext;
            dataContext.DeleteClientCommand.Execute(null);
        }
    }
}
