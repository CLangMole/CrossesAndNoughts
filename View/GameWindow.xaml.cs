using System.Windows;

namespace CrossesAndNoughts.View;

public partial class GameWindow
{
    public GameWindow()
    {
        InitializeComponent();
<<<<<<< HEAD
=======
        DataContext = viewModel;
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
        Closed += (_, _) => Application.Current.Shutdown();
    }
}
