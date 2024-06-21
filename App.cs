using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel;
using System.Windows;

namespace CrossesAndNoughts;

public class App(StartWindow startWindow) : Application
{
<<<<<<< HEAD
=======
    private readonly StartWindow _startWindow;

    public App(StartWindow startWindow, GameWindow gameWindow)
    {
        _startWindow = startWindow;

        AppViewModel.StartWindow = _startWindow;
        AppViewModel.GameWindow = gameWindow;
    }

>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    protected override void OnStartup(StartupEventArgs e)
    {
        startWindow.Show();
        base.OnStartup(e);
    }
}
