using CrossesAndNoughts.ViewModel;
using System.Windows;

namespace CrossesAndNoughts.View;

public partial class StartWindow : Window
{
    public StartWindow(AppViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
