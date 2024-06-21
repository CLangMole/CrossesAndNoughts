using System.Windows;

namespace CrossesAndNoughts.View;

public partial class GameWindow
{
    public GameWindow()
    {
        InitializeComponent();
        Closed += (_, _) => Application.Current.Shutdown();
    }
}
