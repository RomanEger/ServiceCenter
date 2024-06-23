using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using ServiceCenterApp.Models;
using MessageBox = System.Windows.Forms.MessageBox;

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
            if (UserRole.Role == RoleName.EMPLOYEE)
            {
                MessageBox.Show("Удалить может только админ");
                return;
            }
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
            var vm = (WorkViewModel)DataContext;
            if (vm.SelectedWork.StatusId == 3)
            {
                MessageBox.Show("Заявка закрыта");
                return;
            }
            var page = new RequestChange
            {
                DataContext = this.DataContext
            };
            Navigation.Frame.Navigate(page);
        }
    }
}
