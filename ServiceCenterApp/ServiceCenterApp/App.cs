using System.Windows;

namespace ServiceCenterApp;

public class App(Window mainWindow) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        mainWindow.Show(); 
        base.OnStartup(e);
    }
}