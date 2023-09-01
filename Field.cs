using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossesAndNoughts.Symbols;
using CrossesAndNoughts.Symbols.Factories;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CrossesAndNoughts;

public class Field
{
    public void Test()
    {
        SymbolsFactory factory = new CrossesFactory(new Image() { Source = new BitmapImage() { UriSource = new Uri(@"C:\Users\probn\Fiverr\FiverrAssets\OIP.jpg") } });
        Symbol symbol = factory.Create();
        
    }
    

}
