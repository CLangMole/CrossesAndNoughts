using CrossesAndNoughts.View;
using System.Windows;

namespace CrossesAndNoughts;

public class App(StartWindow startWindow) : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        startWindow.Show();
        base.OnStartup(e);
    }
}
