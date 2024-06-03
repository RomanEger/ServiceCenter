using System.Windows;
using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views;

namespace ServiceCenterApp;

public class App(MainWindow mainWindow) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        mainWindow.Show();
        base.OnStartup(e);
    }
}