using CrossesAndNoughts.Models.Strategies;
using System;
using System.Linq;
using System.Threading.Tasks;
using CrossesAndNoughts.Models.Field;

namespace CrossesAndNoughts.Models.Players;

public class User(ISymbolStrategy symbolStrategy, Matrix matrix) : Player(symbolStrategy, matrix.Field)
{
    public event Action? UserDrewSymbol;

    private int _winsCount;

    public override async Task Draw(int row, int column)
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (Buttons is null || !Buttons.Any())
        {
            throw new Exception("There's no button on the field");
        }

        await Task.Yield();

        if (matrix[row, column] != Symbol.Empty)
        {
            return;
        }

        var gameStatus = matrix.GetGameStatus();

        if (gameStatus.IsGameOver)
        {
            if (gameStatus.WinnerSymbol == CurrentSymbol)
            {
                _winsCount += 2;
                SoundsControl.WinSound.Play();
                matrix.DrawWinningLine();
                await Task.Delay(1000);
            }
            else if (gameStatus.WinnerSymbol == Symbol.Empty)
            {
                _winsCount++;
                SoundsControl.WinSound.Play();
            }
            else
            {
                matrix.DrawWinningLine();
                SetButtonsActive(false);
                await Task.Delay(1000);
                SetButtonsActive(true);
            }

            GameOver?.Invoke(_winsCount);

            matrix.Reset();

            return;
        }

        SymbolStrategy.DrawSymbol(matrix, row, column);

        SetButtonsActive(false);

        UserDrewSymbol?.Invoke();
    }
}
