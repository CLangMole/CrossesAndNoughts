using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel;

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
                services.AddSingleton<DBViewModel>();
            })
            .Build();
        var app = host.Services.GetService<App>();
        app?.Run();
    }
}
