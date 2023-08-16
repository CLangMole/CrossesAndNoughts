using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Strategies;

public interface ISymbolStrategy
{ 
    void DrawSymbol(Grid? field, int row, int column);
}

public enum Symbol
{
    Cross,
    Nought,
    Empty
}
