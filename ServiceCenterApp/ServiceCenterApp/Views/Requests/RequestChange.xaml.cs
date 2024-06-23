using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Requests
{
    public partial class RequestChange : Page
    {
        public RequestChange()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var vm = (WorkViewModel)DataContext;
            vm.DeleteFromWorkCommand.Execute(null);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new AddDetailWork()
            {
                DataContext = this.DataContext
            };
            window.ShowDialog();
        }
    }
}
