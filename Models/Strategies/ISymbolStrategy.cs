using CrossesAndNoughts.Models.SymbolsFactories;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Strategies;

public interface ISymbolStrategy
{ 
    void DrawSymbol(Grid? field, int row, int column);
    public Matrix? FieldMatrix { get; set; }
}

public class CrossesStrategy : ISymbolStrategy
{
    public Matrix? FieldMatrix { get; set; }

    private readonly SymbolsFactory _crossesFactory = new CrossesFactory();

    public void DrawSymbol(Grid? field, int row, int column)
    {
        if (FieldMatrix is null)
        {
            throw new System.NullReferenceException(nameof(FieldMatrix));
        }

        Image symbol = _crossesFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        FieldMatrix[row, column] = Symbol.Cross;
    }
}

public class NoughtsStrategy : ISymbolStrategy
{
    public Matrix? FieldMatrix { get; set; }

    private readonly SymbolsFactory _noughtsFactory = new NoughtsFactory();

    public void DrawSymbol(Grid? field, int row, int column)
    {
        if (FieldMatrix is null)
        {
            throw new System.NullReferenceException(nameof(FieldMatrix));
        }

        Image symbol = _noughtsFactory.CreateSymbol();

        field?.Children.Add(symbol);

        symbol.SetValue(Grid.RowProperty, row);
        symbol.SetValue(Grid.ColumnProperty, column);

        FieldMatrix[row, column] = Symbol.Nought;
    }
}

public enum Symbol
{
    Cross,
    Nought,
    Empty
}
