using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.Symbols.Factories;

abstract class NoughtsFactory : SymbolsFactory
{
    private Image _image;
    public NoughtsFactory(Image image) : base(image) => _image = image;

    public override Symbol Create()
    {
        return new Nought();
    }
}
