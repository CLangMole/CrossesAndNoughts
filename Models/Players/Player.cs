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

    public virtual void Draw(int row, int column)
    {
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

    public override void Draw(int row, int column)
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

    public override async void Draw(int row, int column)
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (Buttons is null || !Buttons.Any())
        {
            throw new("There're no buttons on the field");
        }

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
            var miniMax = MiniMax(Matrix.Instance, 4, true);
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

    private Tuple<int, int, int> MiniMax(Matrix matrix, int depth, bool isMaximizing)
    {
        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        int bestRow = -1;
        int bestColumn = -1;

        if (depth == 0 || matrix.GetGameStatus().Item1)
        {
            if (matrix.CurrentUser is null || matrix.CurrentOpponent is null)
            {
                throw new NullReferenceException();
            }

            bestScore = Matrix.Evaluate(matrix);
            return Tuple.Create(bestScore, bestRow, bestColumn);
        }

        Matrix matrixClone = matrix.Copy();

        if (isMaximizing)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == Symbol.Empty)
                    {
                        matrixClone[i, j] = CurrentSymbol;

                        int currentScore = MiniMax(matrixClone, depth - 1, false).Item1;

                        matrixClone.ClearSymbol(i, j);

                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            bestRow = i;
                            bestColumn = j;
                        }
                    }
                }
            }

            return Tuple.Create(bestScore, bestRow, bestColumn);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == Symbol.Empty)
                    {
                        if (Matrix.Instance.CurrentUser is null)
                        {
                            throw new NullReferenceException();
                        }

                        matrixClone[i, j] = Matrix.Instance.CurrentUser.CurrentSymbol;

                        int currentScore = MiniMax(matrixClone, depth - 1, true).Item1;

                        matrixClone.ClearSymbol(i, j);

                        if (currentScore < bestScore)
                        {
                            bestScore = currentScore;
                            bestRow = i;
                            bestColumn = j;
                        }
                    }
                }
            }

            return Tuple.Create(bestScore, bestRow, bestColumn);
        }
    }
}
