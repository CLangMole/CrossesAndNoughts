using CrossesAndNoughts.Models.Strategies;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrossesAndNoughts.Models.Players;

public class Opponent : Player
{
    public event Action? OpponentDrawedSymbol;
    public Symbol CurrentSymbol => _symbol;

    private readonly Symbol _symbol;

    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy)
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

        if (Buttons is null || !Buttons.Any())
        {
            throw new("There're no buttons on the field");
        }

        await Task.Yield();

        SetButtonActive(false);

        await Task.Delay(1000);

        if (Matrix.Instance.GetGameStatus().IsGameOver)
        {
            Matrix.Reset();
            SetButtonActive(true);
            OpponentDrawedSymbol?.Invoke();
            return;
        }

        if (Matrix.Instance.CurrentUser?.CurrentSymbol is null || Matrix.Instance.CurrentOpponent?.CurrentSymbol is null)
        {
            throw new NullReferenceException("You didn't choose your symbol");
        }

        if (row == -1 && column == -1)
        {
            Score miniMax = CurrentSymbol switch
            {
                Symbol.Cross => MiniMax(Matrix.Instance, 4, CurrentSymbol, int.MinValue, int.MaxValue),
                Symbol.Nought => MiniMax(Matrix.Instance, 4, Matrix.Instance.CurrentUser.CurrentSymbol, int.MinValue, int.MaxValue),
                Symbol.Empty => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            
            row = miniMax.BestRow;
            column = miniMax.BestColumn;
        }

        SymbolStrategy?.DrawSymbol(Field, row, column);

        SetButtonActive(true);

        OpponentDrawedSymbol?.Invoke();
    }

    private Score MiniMax(Matrix matrix, int depth, Symbol symbol, int alpha, int beta)
    {
        if (matrix.CurrentUser is null || matrix.CurrentOpponent is null)
        {
            throw new NullReferenceException();
        }

        int bestScore = (symbol == matrix.CurrentUser.CurrentSymbol) ? int.MinValue : int.MaxValue;
        int bestRow = -1;
        int bestColumn = -1;

        if (depth == 0 || matrix.GetGameStatus().IsGameOver)
        {
            bestScore = Matrix.Evaluate(matrix);
        }
        else
        {
            Matrix matrixClone = matrix.Copy();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == Symbol.Empty)
                    {
                        matrixClone[i, j] = symbol;

                        if (symbol == matrix.CurrentUser.CurrentSymbol)
                        {
                            int currentScore = MiniMax(matrixClone, depth - 1, matrix.CurrentOpponent.CurrentSymbol, alpha, beta).BestScore;

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
                            int currentScore = MiniMax(matrixClone, depth - 1, matrix.CurrentUser.CurrentSymbol, alpha, beta).BestScore;

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
        }

        return new Score(bestScore, bestRow, bestColumn);
    }
}
