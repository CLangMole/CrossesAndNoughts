using System.Windows;
using CrossesAndNoughts.ViewModel;

namespace CrossesAndNoughts.View;

public partial class StartWindow : Window
{
    public StartWindow(AppViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}    
