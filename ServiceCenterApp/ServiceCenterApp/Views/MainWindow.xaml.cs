using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views.Clients;
using ServiceCenterApp.Views.Reports;
using ServiceCenterApp.Views.Requests;
using ServiceCenterApp.Views.Stock;
using System.Windows;

namespace ServiceCenterApp.Views;

public partial class MainWindow : Window
{
    private static ServiceCenterDbContext _dbContext { get; set; }

    public static ServiceCenterDbContext DbContext { get => _dbContext; set => _dbContext = value; }

    public static Employee Employee { get; set; }

    private readonly App _app;
    
    public MainWindow(ServiceCenterDbContext dbContext, App app)
    {
        InitializeComponent();
        _app = app;
        WindowState = WindowState.Maximized;
        DbContext = dbContext;
        var authWindow = new AuthWindow(dbContext);
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

    private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
    {
        UserRole.Role = null;
        this.Hide();
        _app.StartUp();
        this.Close();
    }
}