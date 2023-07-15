using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Players;

public abstract class Player
{
    protected readonly IEnumerable<Button>? Buttons = Field?.Children.OfType<Button>();
    protected ISymbolStrategy? SymbolStrategy { get => _symbolStrategy; }

    public static Grid? Field { get; set; }

    private readonly ISymbolStrategy _symbolStrategy;

    public Player(ISymbolStrategy symbolStrategy)
    {
        _symbolStrategy = symbolStrategy;
    }

    public virtual async Task Draw(int row, int column)
    {
        await Task.Yield();
        _symbolStrategy.DrawSymbol(null, row, column);
    }
}

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

        SymbolStrategy.DrawSymbol(Field, row, column);

        foreach (Button? button in Buttons)
        {
            button.IsEnabled = false;
        }


        UserDrawedSymbol?.Invoke();
    }
}

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

        bool isGameOver = Matrix.Instance.GetGameStatus().Item1;
        Symbol winner = Matrix.Instance.GetGameStatus().Item2;

        if (isGameOver)
        {
            if (winner == Symbol.Empty)
            {
                MessageBox.Show("Game over! It's a draw!");
            }
            else
            {
                MessageBox.Show($"Game over! {winner} wins!");
            }

            return;
        }

        SetButtonActive(false);

        await Task.Delay(1000);

        if (Matrix.Instance.CurrentUser?.CurrentSymbol is null || Matrix.Instance.CurrentOpponent?.CurrentSymbol is null)
        {
            throw new NullReferenceException("You didn't choose your symbol");
        }

        if (row == -1 && column == -1)
        {
            var miniMax = MiniMax(Matrix.Instance, 4, Matrix.Instance.CurrentUser.CurrentSymbol, int.MinValue, int.MaxValue);
            row = miniMax.Item2;
            column = miniMax.Item3;
        }

        SymbolStrategy?.DrawSymbol(Field, row, column);

        SetButtonActive(true);

        OpponentDrawedSymbol?.Invoke();
    }

    private void SetButtonActive(bool isEnabled)
    {
        if (Buttons is null || !Buttons.Any())
        {
            throw new Exception("Buttons array is null or empty");
        }

        foreach (Button button in Buttons)
        {
            button.IsEnabled = isEnabled;
        }
    }

    private Tuple<int, int, int> MiniMax(Matrix matrix, int depth, Symbol symbol, int alpha, int beta)
    {
        if (matrix.CurrentUser is null || matrix.CurrentOpponent is null)
        {
            throw new NullReferenceException();
        }

        int bestScore = (symbol == matrix.CurrentUser.CurrentSymbol) ? int.MinValue : int.MaxValue;
        int bestRow = -1;
        int bestColumn = -1;

        if (depth == 0 || matrix.GetGameStatus().Item1)
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
                            int currentScore = MiniMax(matrixClone, depth - 1, matrix.CurrentOpponent.CurrentSymbol, alpha, beta).Item1;

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
                            int currentScore = MiniMax(matrixClone, depth - 1, matrix.CurrentUser.CurrentSymbol, alpha, beta).Item1;

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

        return Tuple.Create(bestScore, bestRow, bestColumn);
    }
}
