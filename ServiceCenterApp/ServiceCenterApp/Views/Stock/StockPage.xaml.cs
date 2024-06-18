using ServiceCenterApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ServiceCenterApp.Views.Stock
{
    /// <summary>
    /// Логика взаимодействия для Stock.xaml
    /// </summary>
    public partial class StockPage : Page
    {
        public StockPage()
        {
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (StockViewModel)DataContext;
            dataContext.DeleteCommand.Execute(null);
        }

        private void BtnChange_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new StockChange()
            {
                DataContext = this.DataContext
            };
            window.ShowDialog();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new DetailAddOrUpdate()
            {
                DataContext = this.DataContext
            };
            window.ShowDialog();
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new StockAdd()
            {
                DataContext = this.DataContext
            };
            window.ShowDialog();
        }
    }
}
