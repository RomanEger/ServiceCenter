using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Requests
{
    /// <summary>
    /// Логика взаимодействия для RequestsList.xaml
    /// </summary>
    public partial class RequestsList : Page
    {
        public RequestsList()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (WorkViewModel)DataContext;
            dataContext.DeleteCommand.Execute(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var createRequestPage = new CreateRequest
            {
                DataContext = this.DataContext
            };
            Navigation.Frame.Navigate(createRequestPage);
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            var page = new RequestChange
            {
                DataContext = this.DataContext
            };
            Navigation.Frame.Navigate(page);
        }
    }
}
