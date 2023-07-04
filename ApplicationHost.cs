using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CrossesAndNoughts;

public class ApplicationHost
{
    [STAThread]
    public static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<App>();
                services.AddSingleton<StartWindow>();
                services.AddSingleton<GameWindow>();
                services.AddSingleton<AppViewModel>();
            })
            .Build();
        var app = host.Services.GetService<App>();
        app?.Run();
    }
}
