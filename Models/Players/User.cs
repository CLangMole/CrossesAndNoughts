using CrossesAndNoughts.Models.Strategies;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Players;

public class User : Player
{
    public event Action? UserDrawedSymbol;
    public Symbol CurrentSymbol => _symbol;

    private readonly Symbol _symbol;

    public User(ISymbolStrategy symbolStrategy) : base(symbolStrategy)
    {
        _symbol = symbolStrategy switch
        {
            CrossesStrategy => Symbol.Cross,
            NoughtsStrategy => Symbol.Nought,
            _ => throw new NotImplementedException()
        };
    }

    public override async Task Draw(int row, int column)
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (Field is null)
        {
            throw new NullReferenceException(nameof(Field));
        }

        if (Buttons is null || !Buttons.Any())
        {
            throw new Exception("There's no button on the field");
        }

        await Task.Yield();

        if (Matrix.Instance.GetGameStatus().IsGameOver)
        {
            Matrix.Reset();
            Debug.WriteLine(Matrix.Instance.GetGameStatus().WinnerSymbol);
            return;
        }

        SymbolStrategy.DrawSymbol(Field, row, column);

        SetButtonActive(false);

        UserDrawedSymbol?.Invoke();
    }
}
