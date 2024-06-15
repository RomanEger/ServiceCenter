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
    }
}
