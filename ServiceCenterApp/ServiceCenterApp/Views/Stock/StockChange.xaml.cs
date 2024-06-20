using System.Windows;
using ServiceCenterApp.ViewModels;

namespace ServiceCenterApp.Views.Stock;

public partial class StockChange : Window
{
    public StockChange()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = (StockViewModel)DataContext;
        vm.SaveChangesCommand.Execute(null);
        this.Close();
    }

    private void StockChange_OnLoaded(object sender, RoutedEventArgs e)
    {
        var vm = (StockViewModel)DataContext;
        vm.StockDetail = vm.GetStockDetail();
    }
}