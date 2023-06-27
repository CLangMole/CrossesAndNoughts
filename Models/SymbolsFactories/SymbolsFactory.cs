using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CrossesAndNoughts.Models.SymbolsFactories;

public abstract class SymbolsFactory
{
    public abstract Image CreateSymbol();

    protected static void SetStyle(Image image)
    {
        image.Visibility = Visibility.Visible;
        image.Margin = new Thickness(10);
    }
}

public class CrossesFactory : SymbolsFactory
{
    public override Image CreateSymbol()
    {
        var image = new Image { Source = BitmapFrame.Create(new Uri(@"C:\Users\probn\Fiverr\FiverrAssets\Images\Cross.png")) };
        SetStyle(image);
        return image;
    }
}

public class NoughtsFactory : SymbolsFactory
{
    public override Image CreateSymbol()
    {
        var image = new Image { Source = BitmapFrame.Create(new Uri(@"C:\Users\probn\Fiverr\FiverrAssets\Images\Nought.png")) };
        SetStyle(image);
        return image;
    }
}
