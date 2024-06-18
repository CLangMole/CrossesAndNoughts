using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;
using CrossesAndNoughts.Models.Field;

namespace CrossesAndNoughts.Models.Strategies;

public class NoughtsStrategy : ISymbolStrategy
{
    private readonly SymbolsFactory _noughtsFactory = new NoughtsFactory();

    public Symbol PlayerSymbol => Symbol.Nought;

    public void DrawSymbol(Matrix matrix, int row, int column)
    {
        var symbol = _noughtsFactory.CreateSymbol();

        matrix.Field.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        matrix[row, column] = Symbol.Nought;

        SoundsControl.ClickSound.Play();
    }
}
