using System.Windows;
using ServiceCenterApp.Models;
using ServiceCenterApp.Views;

namespace ServiceCenterApp;

public class App(ServiceCenterDbContext dbContext) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = new MainWindow(dbContext);
        mainWindow.Show();
        base.OnStartup(e);
    }
}