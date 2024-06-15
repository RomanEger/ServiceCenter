using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ServiceCenterApp.Models;
using ServiceCenterApp.ViewModels;

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
            })
            .Build();
        
        var app = host.Services.GetRequiredService<App>();
        app.Run();
    }
}