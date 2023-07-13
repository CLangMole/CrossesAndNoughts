using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        switch (symbolStrategy)
        {
            case CrossesStrategy:
                _symbol = Symbol.Cross;
                break;
            case NoughtsStrategy:
                _symbol = Symbol.Nought;
                break;
        }
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
    private readonly Random _random = new();

    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy)
    {
        switch (symbolStrategy)
        {
            case CrossesStrategy:
                _symbol = Symbol.Cross;
                break;
            case NoughtsStrategy:
                _symbol = Symbol.Nought;
                break;
        }
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

        SetButtonActive(false);

        await Task.Delay(1000);

        if (Matrix.Instance.CurrentUser?.CurrentSymbol is null || Matrix.Instance.CurrentOpponent?.CurrentSymbol is null)
        {
            throw new NullReferenceException("You didn't choose your symbol");
        }

        row = MiniMax(Matrix.Instance, 4, Matrix.Instance.CurrentUser.CurrentSymbol).Item2;
        column = MiniMax(Matrix.Instance, 4, Matrix.Instance.CurrentUser.CurrentSymbol).Item3;

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

    private Tuple<int, int, int> MiniMax(Matrix matrix, int depth, Symbol symbol)
    {
        if (symbol == Symbol.Empty)
        {
            throw new ArgumentException("Symbol cannot be empty", nameof(symbol));
        }

        int bestScore = symbol == Matrix.Instance.CurrentUser?.CurrentSymbol ? int.MinValue : int.MaxValue;
        int bestRow = -1;
        int bestColumn = -1;

        if (depth == 0 || matrix.GetGameStatus().Item1)
        {
            bestScore = Matrix.Instance.Evaluate();
        }

        Matrix matrixClone = matrix.Copy();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (matrix[i, j] == Symbol.Empty)
                {
                    matrixClone[i, j] = symbol;

                    if (symbol == Matrix.Instance.CurrentUser?.CurrentSymbol)
                    {
                        int currentScore = MiniMax(matrixClone, depth - 1, CurrentSymbol).Item1;

                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            bestRow = i;
                            bestColumn = j;
                        }
                    }
                    else
                    {
                        int currentScore = MiniMax(matrixClone, depth - 1, Matrix.Instance.CurrentUser.CurrentSymbol).Item1;

                        if (currentScore < bestScore)
                        {
                            bestScore = currentScore;
                            bestRow = i;
                            bestColumn = j;
                        }
                    }

                    matrixClone.ClearSymbol(i, j);
                }
            }
        }

        return Tuple.Create(bestScore, bestRow, bestColumn);
    }
}
