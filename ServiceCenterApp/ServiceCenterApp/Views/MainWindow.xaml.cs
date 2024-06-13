using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views.Clients;
using ServiceCenterApp.Views.Reports;
using ServiceCenterApp.Views.Requests;
using ServiceCenterApp.Views.Stock;
using System.Windows;
using System.Windows.Input;

namespace ServiceCenterApp.Views;

public partial class MainWindow : Window
{
    private static ServiceCenterDbContext _dbContext { get; set; }

    public static ServiceCenterDbContext DbContext { get => _dbContext; set => _dbContext = value; }

    public MainWindow(ServiceCenterDbContext dbContext, AuthWindow authWindow)
    {
        InitializeComponent();
        WindowState = WindowState.Maximized;
        DbContext = dbContext;
        authWindow.ShowDialog();
        if (UserRole.Role == null)
        {
            //все норм
            throw new Exception();
        }
        //добавить авторизацию
        Navigation.Frame = MainFrame;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var requestPage = new RequestsList();
        requestPage.DataContext = new WorkViewModel(_dbContext);
        Navigation.Frame.Navigate(requestPage);
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        var clientsPage = new ClientsList();
        clientsPage.DataContext = new ClientViewModel(_dbContext);
        Navigation.Frame.Navigate(clientsPage);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        var stockPage = new StockPage();
        stockPage.DataContext = new StockViewModel(_dbContext);
        Navigation.Frame.Navigate(stockPage);
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        var reportsPage = new ReportsCreate();
        reportsPage.DataContext = new ReportViewModel(_dbContext);
        Navigation.Frame.Navigate(reportsPage);
    }
}