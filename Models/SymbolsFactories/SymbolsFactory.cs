using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CrossesAndNoughts.Models.SymbolsFactories;

public abstract class SymbolsFactory
{
    public abstract Image CreateSymbol();

    protected static readonly string CrossPath = @"pack://application:,,,/Resources/Images/Cross3.png";
    protected static readonly string NoughtPath = @"pack://application:,,,/Resources/Images/Nought5.png";

    protected static Image CustomizedSymbol(string path)
    {
        return new Image()
        {
            Source = BitmapFrame.Create(new Uri(path, UriKind.Absolute)),
            Visibility = Visibility.Visible,
            Margin = new Thickness(10)
        };
    }
}
