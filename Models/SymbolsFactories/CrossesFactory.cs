using System.Windows.Controls;

namespace CrossesAndNoughts.Models.SymbolsFactories;

public class CrossesFactory : SymbolsFactory
{
    public override Image CreateSymbol()
    {
        return CustomizedSymbol(CrossPath);
    }
}
