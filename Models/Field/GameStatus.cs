using CrossesAndNoughts.Models.Strategies;

namespace CrossesAndNoughts.Models.Field;

public class GameStatus(bool isGameOver, Symbol winnerSymbol)
{
    public Symbol WinnerSymbol { get; } = winnerSymbol;

    public bool IsGameOver { get; } = isGameOver;
}