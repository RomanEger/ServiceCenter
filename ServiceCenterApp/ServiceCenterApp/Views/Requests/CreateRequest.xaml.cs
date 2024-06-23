using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views.Clients;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Requests
{
    public partial class CreateRequest : Page
    {
        public CreateRequest()
        {
            InitializeComponent();
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            var createClientPage = new ClientRegistrationWindow();
            createClientPage.DataContext = new ClientViewModel(MainWindow.DbContext);
            createClientPage.ShowDialog();
        }
    }
}
