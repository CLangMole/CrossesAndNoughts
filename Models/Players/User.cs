using CrossesAndNoughts.Models.Strategies;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CrossesAndNoughts.Models.Players;

public class User : Player
{
    public event Action? UserDrawedSymbol;
    public Symbol CurrentSymbol => _symbol;

    private readonly Symbol _symbol;

    private int _winsCount = 0;

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

        var gameStatus = Matrix.Instance.GetGameStatus();

        if (gameStatus.IsGameOver)
        {
            if (gameStatus.WinnerSymbol == _symbol)
            {
                _winsCount += 2;
                SoundsControl.WinSound.Play();
                Matrix.DrawWinningLine();
                await Task.Delay(1000);
            }
            else if (gameStatus.WinnerSymbol == Symbol.Empty)
            {
                _winsCount++;
                SoundsControl.WinSound.Play();
            }
            else
            {
                Matrix.DrawWinningLine();
                SetButtonsActive(false);
                await Task.Delay(1000);
            }

            GameOver?.Invoke(_winsCount);

            Matrix.Reset();

            return;
        }

        SymbolStrategy.DrawSymbol(Field, row, column);

        SetButtonsActive(false);

        UserDrawedSymbol?.Invoke();
    }
}
