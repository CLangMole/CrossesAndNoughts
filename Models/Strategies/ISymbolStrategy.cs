using CrossesAndNoughts.Models.Field;

namespace CrossesAndNoughts.Models.Strategies;

public interface ISymbolStrategy
{
    void DrawSymbol(Matrix matrix, int row, int column);
    
    Symbol PlayerSymbol { get; }
}

public enum Symbol
{
    Empty,
    Cross,
    Nought
}
