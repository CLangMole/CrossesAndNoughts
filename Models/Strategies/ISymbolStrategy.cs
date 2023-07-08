using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Strategies;

public interface ISymbolStrategy
{ 
    void DrawSymbol(Grid? field, int row, int column);
}

public class CrossesStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _crossesFactory = new CrossesFactory();

    public void DrawSymbol(Grid? field, int row, int column)
    {
        Image symbol = _crossesFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        Matrix.Instance.AddItem(symbol, row, column);
    }
}

public class NoughtsStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _noughtsFactory = new NoughtsFactory();

    public void DrawSymbol(Grid? field, int row, int column)
    {
        Image symbol = _noughtsFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        Matrix.Instance.AddItem(symbol, row, column);
    }
}

public enum Symbol
{
    Cross,
    Nought
}
