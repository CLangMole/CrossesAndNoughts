using CrossesAndNoughts.ViewModel;
using System.Windows;

namespace CrossesAndNoughts.View;

/// <summary>
/// Interaction logic for GameWindow.xaml
/// </summary>
public partial class GameWindow : Window
{
    public GameWindow(AppViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Closed += (sender, e) => Application.Current.Shutdown();
    }
}
