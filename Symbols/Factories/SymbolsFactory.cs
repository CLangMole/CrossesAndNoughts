using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.Symbols.Factories;

abstract class SymbolsFactory
{
    private Image _image;
    public SymbolsFactory(Image image) => _image = image;

    public abstract Symbol Create();
}
