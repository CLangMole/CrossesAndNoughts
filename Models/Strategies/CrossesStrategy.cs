using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Strategies;

public class CrossesStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _crossesFactory = new CrossesFactory();

    public void DrawSymbol(Grid? field, int row, int column)
    {
        Image symbol = _crossesFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        Matrix.Instance[row, column] = Symbol.Cross;
        SoundsControl.ClickSound.Play();
    }
}
