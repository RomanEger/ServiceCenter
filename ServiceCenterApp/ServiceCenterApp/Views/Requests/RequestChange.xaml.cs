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
    /// Логика взаимодействия для RequestChange.xaml
    /// </summary>
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
