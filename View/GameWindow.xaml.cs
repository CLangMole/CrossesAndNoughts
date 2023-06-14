using System.Windows;

namespace CrossesAndNoughts.View;

/// <summary>
/// Interaction logic for GameWindow.xaml
/// </summary>
public partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();
        Closed += (sender, e) => Application.Current.Shutdown();
    }
}
