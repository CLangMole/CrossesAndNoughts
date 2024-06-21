using System.Windows;

namespace CrossesAndNoughts.View;

public partial class StartWindow
{
    public StartWindow()
    {
        InitializeComponent();
        Closed += (_, _) => Application.Current.Shutdown();
    }
}
