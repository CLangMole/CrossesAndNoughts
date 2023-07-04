using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel;
using System.Windows;

namespace CrossesAndNoughts;

public class App : Application
{
    private readonly StartWindow _startWindow;
    private readonly GameWindow _gameWindow;

    public App(StartWindow startWindow, GameWindow gameWindow)
    {
        _startWindow = startWindow;
        _gameWindow = gameWindow;

        AppViewModel.StartWindow = _startWindow;
        AppViewModel.GameWindow = _gameWindow;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        _startWindow.Show();
        base.OnStartup(e);
    }
}
