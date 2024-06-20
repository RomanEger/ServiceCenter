using System.Windows;
using ServiceCenterApp.Models;
using ServiceCenterApp.Views;

namespace ServiceCenterApp;

public class App(ServiceCenterDbContext dbContext) : Application
{
    public void StartUp()
    {
        OnStartup(null);
    }
    protected override void OnStartup(StartupEventArgs e)
    {
        var mainWindow = new MainWindow(dbContext, this);
        mainWindow.Show();
        base.OnStartup(e);
    }
}