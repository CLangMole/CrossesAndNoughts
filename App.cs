using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel;
using System.Windows;

namespace CrossesAndNoughts;

public class App : Application
{
    private readonly StartWindow _startWindow;

    public App(StartWindow startWindow, GameWindow gameWindow)
    {
        _startWindow = startWindow;

        AppViewModel.StartWindow = _startWindow;
        AppViewModel.GameWindow = gameWindow;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _startWindow.Show();
        base.OnStartup(e);
    }
}
