using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public static Matrix? FieldMatrix { get; set; }

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

        SymbolStrategy.FieldMatrix = FieldMatrix;

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
    public int Points => _points;
    public Symbol CurrentSymbol => _symbol;

    private readonly Symbol _symbol;
    private int _points = 0;

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

        if (FieldMatrix is null)
        {
            throw new NullReferenceException(nameof(FieldMatrix));
        }

        await Task.Yield();

        bool isGameOver = FieldMatrix.GetGameStatus().IsGameOver;
        Symbol winner = FieldMatrix.GetGameStatus().WinnerSymbol;

        if (isGameOver)
        {
            if (winner == Symbol.Empty)
            {
                MessageBox.Show("Game over! It's a draw!");
            }
            else
            {
                MessageBox.Show($"Game over! {winner} wins!");

                if (winner == CurrentSymbol)
                {
                    _points--;
                }
                else
                {
                    _points++;
                }

                Debug.WriteLine(_points);
            }

            return;
        }

        SetButtonActive(false);

        await Task.Delay(1000);

        if (FieldMatrix.CurrentUser?.CurrentSymbol is null || FieldMatrix.CurrentOpponent?.CurrentSymbol is null)
        {
            throw new NullReferenceException("You didn't choose your symbol");
        }

        if (row == -1 && column == -1)
        {
            Score miniMax = CurrentSymbol switch
            {
                Symbol.Cross => MiniMax(FieldMatrix, 4, CurrentSymbol, int.MinValue, int.MaxValue),
                Symbol.Nought => MiniMax(FieldMatrix, 4, FieldMatrix.CurrentUser.CurrentSymbol, int.MinValue, int.MaxValue),
                Symbol.Empty => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            
            row = miniMax.BestRow;
            column = miniMax.BestColumn;
        }

        SymbolStrategy.FieldMatrix = FieldMatrix;

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
