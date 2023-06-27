using System.Windows;
using CrossesAndNoughts.ViewModel;

namespace CrossesAndNoughts.View;

public partial class StartWindow : Window
{
    public StartWindow(DBViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}    
