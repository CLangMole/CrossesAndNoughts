using System.Windows.Controls;

namespace CrossesAndNoughts.Models.SymbolsFactories;

public class NoughtsFactory : SymbolsFactory
{
    public override Image CreateSymbol()
    {
        return CustomizedSymbol(NoughtPath);
    }
}
