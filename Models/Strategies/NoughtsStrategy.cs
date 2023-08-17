using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Strategies;

public class NoughtsStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _noughtsFactory = new NoughtsFactory();

    public void DrawSymbol(Grid? field, int row, int column)
    {
        Image symbol = _noughtsFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        Matrix.Instance[row, column] = Symbol.Nought;

        SoundsControl.ClickSound.Play();
    }
}
