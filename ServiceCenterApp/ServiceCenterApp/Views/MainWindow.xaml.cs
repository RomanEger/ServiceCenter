using ServiceCenterApp.Models;
using System.Windows;
using System.Windows.Input;

namespace ServiceCenterApp.Views;

public partial class MainWindow : Window
{
    private bool IsMaximized { get; set; }

    private ServiceCenterDbContext _dbContext { get;}

    public MainWindow(ServiceCenterDbContext dbContext, AuthWindow authWindow)
    {
        InitializeComponent();
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        WindowState = WindowState.Maximized;
        IsMaximized = true;
        _dbContext = dbContext;
        authWindow.ShowDialog();
        Navigation.Frame = MainFrame;
    }

    private void Exit(object sender, RoutedEventArgs e) => Environment.Exit(0);

    private void ChangeState(object sender, RoutedEventArgs e)
    {
        if (IsMaximized)
        {
            WindowState = WindowState.Normal;
            IsMaximized = false;
        }
        else
        {
            WindowState = WindowState.Maximized;
            IsMaximized = true;
        }
    }

    private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs) => DragMove();
}