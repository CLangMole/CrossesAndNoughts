using CrossesAndNoughts.Models.Strategies;

namespace CrossesAndNoughts.Models.Field;

public readonly struct GameStatus(bool isGameOver, Symbol winnerSymbol)
{
    public Symbol WinnerSymbol { get; } = winnerSymbol;

    public bool IsGameOver { get; } = isGameOver;
}