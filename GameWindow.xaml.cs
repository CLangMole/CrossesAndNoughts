using CrossesAndNoughts.Symbols.Factories;
using CrossesAndNoughts.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrossesAndNoughts;

/// <summary>
/// Interaction logic for GameWindow.xaml
/// </summary>
public partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();
        Closed += (sender, e) => Application.Current.Shutdown();

        SymbolsFactory factory = new CrossesFactory(new Image() { Source = new BitmapImage() { UriSource = new Uri(@"C:\Users\probn\Fiverr\FiverrAssets\OIP.jpg") } });
        Symbol symbol = factory.Create();
        Field.Children.Add(new Image() { Source = new BitmapImage() { UriSource = new Uri(@"C:\Users\probn\Fiverr\FiverrAssets\OIP.jpg") } });
    }
}
