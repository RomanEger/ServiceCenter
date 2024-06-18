using System.Windows;
using ServiceCenterApp.ViewModels;

namespace ServiceCenterApp.Views.Requests;

public partial class AddDetailWork : Window
{
    public AddDetailWork()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var vm = (WorkViewModel)DataContext;
        vm.AddDetailWorkCommand.Execute(null);
        this.Close();
    }
}