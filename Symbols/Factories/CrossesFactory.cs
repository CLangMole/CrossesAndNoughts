using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.Symbols.Factories;

class CrossesFactory : SymbolsFactory
{
    private Image _image;
    public CrossesFactory(Image image) : base(image) => _image = image;

    public override Symbol Create()
    {
        return new Cross();
    }
}
