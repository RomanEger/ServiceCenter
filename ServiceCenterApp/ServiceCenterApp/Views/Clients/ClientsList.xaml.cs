using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Clients
{
    /// <summary>
    /// Логика взаимодействия для ClientsList.xaml
    /// </summary>
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
            var dataContext = (ClientViewModel)DataContext;
            dataContext.DeleteClientCommand.Execute(null);
        }
    }
}
