using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;
using ServiceCenterApp.Views;

namespace ServiceCenterApp;
 
public static class Program
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                services.AddScoped<AuthViewModel>();
                services.AddSingleton<IConfigurationBuilder, ConfigurationBuilder>();
                services.AddDbContext<ServiceCenterDbContext>();
                services.AddSingleton<MainWindow>();
                services.AddSingleton<AuthWindow>();
            })
            .Build();
        
        var app = host.Services.GetRequiredService<App>();
        app.Run();
    }
}