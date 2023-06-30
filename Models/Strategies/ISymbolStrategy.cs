using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Strategies;

public interface ISymbolStrategy
{
    void DrawSymbol(Grid? field, int rowIndex, int columnIndex);
}

public class CrossesStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _crossesFactory = new CrossesFactory();

    public void DrawSymbol(Grid? field, int rowIndex, int columnIndex)
    {
        Image symbol = _crossesFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, rowIndex);
        symbol.SetValue(Grid.ColumnProperty, columnIndex);
    }
}

public class NoughtsStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _noughtsFactory = new NoughtsFactory();

    public void DrawSymbol(Grid? field, int rowIndex, int columnIndex)
    {
        Image symbol = _noughtsFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, rowIndex);
        symbol.SetValue(Grid.ColumnProperty, columnIndex);
    }
}
