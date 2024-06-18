using System.Windows;
using ServiceCenterApp.ViewModels;

namespace ServiceCenterApp.Views.Stock;

public partial class DetailAddOrUpdate : Window
{
    public DetailAddOrUpdate()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = (StockViewModel)DataContext;
        vm.AddOrUpdateDetailCommand.Execute(null);
        this.Close();
    }
}