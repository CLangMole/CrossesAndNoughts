using CrossesAndNoughts.ViewModel;
using System.Windows;

namespace CrossesAndNoughts.View;

public partial class GameWindow
{
    public GameWindow(AppViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Closed += (_, _) => Application.Current.Shutdown();
    }
}
