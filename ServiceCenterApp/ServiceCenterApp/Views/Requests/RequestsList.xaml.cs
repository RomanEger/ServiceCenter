using ServiceCenterApp.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var createRequestPage = new CreateRequest();
            createRequestPage.DataContext = this.DataContext;
            Navigation.Frame.Navigate(createRequestPage);
        }
    }
}
