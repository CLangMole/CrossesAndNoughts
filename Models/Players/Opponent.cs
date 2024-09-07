using CrossesAndNoughts.Models.Strategies;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using CrossesAndNoughts.Models.Field;
using Matrix = CrossesAndNoughts.Models.Field.Matrix;

namespace CrossesAndNoughts.Models.Players;

public class Opponent(ISymbolStrategy symbolStrategy, Matrix matrix) : Player(symbolStrategy, matrix.Field)
{
    public (Brush, string) CurrentDifficulty { get; private set; } = (Brushes.YellowGreen, "Easy");

    private int _pointsCount;
    private int _difficulty = 1;

    public override async Task Draw(int row, int column)
    {
        if (Buttons is null || !Buttons.Any())
        {
            throw new Exception("There are no buttons on the field");
        }

        SetButtonsActive(false);

        await Task.Delay(1000);

        var gameStatus = matrix.GetGameStatus();

        if (gameStatus.IsGameOver)
        {
            await CheckGameStatus(gameStatus);
            return;
        }

        if (row == -1 && column == -1)
        {
            var miniMax = MiniMax(matrix, _difficulty, CurrentSymbol, int.MinValue, int.MaxValue);

            row = miniMax.BestRow;
            column = miniMax.BestColumn;
        }

        SymbolStrategy.DrawSymbol(matrix, row, column);

        SetButtonsActive(true);

        var gameStatusAfterDraw = matrix.GetGameStatus();

        await Task.Delay(100);
        
        if (gameStatusAfterDraw.IsGameOver)
        {
            await CheckGameStatus(gameStatusAfterDraw);
        }
    }

    private async Task CheckGameStatus(GameStatus gameStatus)
    {
        if (gameStatus.WinnerSymbol != CurrentSymbol && gameStatus.WinnerSymbol != Symbol.Empty)
        {
            _pointsCount += 2;
            SoundsControl.WinSound.Play();
            matrix.DrawWinningLine();
            await Task.Delay(1000);
        }
        else if (gameStatus.WinnerSymbol == Symbol.Empty)
        {
            _pointsCount++;
            SoundsControl.WinSound.Play();
        }
        else
        {
            matrix.DrawWinningLine();
            SetButtonsActive(false);
            await Task.Delay(1000);
            SetButtonsActive(true);
        }

        if (gameStatus.WinnerSymbol != CurrentSymbol)
        {
            if (_difficulty < 4)
            {
                _difficulty++;
            }

            CurrentDifficulty = _difficulty switch
            {
                0 => (Brushes.AliceBlue, "TooEasy"),
                1 => (Brushes.GreenYellow, "Easy"),
                2 => (Brushes.Yellow, "Middle"),
                3 => (Brushes.DarkOrange, "Hard"),
                4 => (Brushes.Red, "Insane"),
                _ => throw new ArgumentException("Invalid symbol")
            };
            
            matrix.Reset();
        }

        GameOver?.Invoke(_pointsCount);

        if (CurrentSymbol == Symbol.Cross)
        {
            SetButtonsActive(false);
            var draw = Draw(-1, -1);
            draw.Start();

            return;
        }

        SetButtonsActive(true);
    }

    private static Score MiniMax(Matrix matrix, int depth, Symbol symbol, int alpha, int beta)
    {
        var bestScore = symbol == matrix.UserSymbol ? int.MinValue : int.MaxValue;
        var bestRow = -1;
        var bestColumn = -1;

        if (depth == 0 || matrix.GetGameStatus().IsGameOver)
        {
            bestScore = matrix.Evaluate();
        }
        else
        {
            var matrixClone = matrix.Copy();

            for (var i = 0; i < matrix.Size; i++)
            {
                for (var j = 0; j < matrix.Size; j++)
                {
                    if (matrix[i, j] != Symbol.Empty)
                    {
                        continue;
                    }

                    matrixClone[i, j] = symbol;

                    if (symbol == matrix.UserSymbol)
                    {
                        var currentScore = MiniMax(matrixClone, depth - 1, matrix.OpponentSymbol, alpha, beta)
                            .BestScore;

                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            bestRow = i;
                            bestColumn = j;
                        }

                        alpha = Math.Max(alpha, bestScore);

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var currentScore = MiniMax(matrixClone, depth - 1, matrix.UserSymbol, alpha, beta).BestScore;

                        if (currentScore < bestScore)
                        {
                            bestScore = currentScore;
                            bestRow = i;
                            bestColumn = j;
                        }

                        beta = Math.Min(beta, bestScore);

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }

                    matrixClone.ClearSymbol(i, j);
                }
            }
        }

        return new Score(bestScore, bestRow, bestColumn);
    }
}