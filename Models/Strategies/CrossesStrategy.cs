using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;
using CrossesAndNoughts.Models.Field;

namespace CrossesAndNoughts.Models.Strategies;

public class CrossesStrategy : ISymbolStrategy
{
    public Symbol PlayerSymbol => Symbol.Cross;
    
    private readonly SymbolsFactory _crossesFactory = new CrossesFactory();

    public void DrawSymbol(Matrix matrix, int row, int column)
    {
        var symbol = _crossesFactory.CreateSymbol();

        matrix.Field.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        matrix[row, column] = Symbol.Cross;
        SoundsControl.ClickSound.Play();
    }
}
