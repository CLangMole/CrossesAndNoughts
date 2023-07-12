using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    public Symbol Symbol => _symbol;

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
    public Symbol Symbol => _symbol;

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

        //for (int i = 0; i < 3; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        if (Matrix.Instance[1, 1] == Symbol.Empty)
        //        {
        //            row = 1;
        //            column = 1;
        //        }
        //        else if (Matrix.Instance[i, j] == Symbol.Empty)
        //        {
        //            row = _random.Next(0, i);
        //            column = _random.Next(0, j);
        //        }
        //    }
        //}

        //row = BestCell().Item1;
        //column = BestCell().Item2;

        //SymbolStrategy.DrawSymbol(Field, row, column);

        int bestScore = int.MinValue;
        int bestRow = -1;
        int bestColumn = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Matrix.Instance[i, j] == Symbol.Empty)
                {
                    SymbolStrategy.DrawSymbol(Field, i, j);

                    int score = MiniMax(0, false);

                    Matrix.Instance[i, j] = Symbol.Empty;

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestRow = i;
                        bestColumn = j;
                    }
                }
            }
        }

        SymbolStrategy.DrawSymbol(Field, bestRow, bestColumn);

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

    //private static Tuple<int, int> BestCell()
    //{
    //    int row = 0;
    //    int column = 0;

    //    Queue<Tuple<int, int>> queue = new();
    //    queue.Enqueue(Tuple.Create(1, 1));

    //    while (queue.Count != 0)
    //    {
    //        var cell = queue.Dequeue();

    //        if (Matrix.Instance[cell.Item1, cell.Item2] != Symbol.Empty)
    //        {
    //            continue;
    //        }

    //        for (int i = -1; i <= 1; i++)
    //        {
    //            for (int j = -1; j <= 1; j++)
    //            {
    //                if (i != 0 && j != 0)
    //                {
    //                    continue;
    //                }
    //                else
    //                {
    //                    queue.Enqueue(Tuple.Create(row = cell.Item1 + i, column = cell.Item2 + j));
    //                }
    //            }
    //        }
    //    }

    //    return Tuple.Create(row, column);
    //}

    protected int MiniMax(int startDepth, bool isMaximizingPlayer)
    {
        if (Matrix.Instance.IsGameOver())
        {
            Symbol winnerSymbol = Matrix.Instance.Winner();

            switch (Symbol)
            {
                case Symbol.Nought:
                    return winnerSymbol switch
                    {
                        Symbol.Cross => startDepth - 10,
                        Symbol.Nought => 10 - startDepth,
                        _ => 0,
                    };
                case Symbol.Cross:
                    return winnerSymbol switch
                    {
                        Symbol.Nought => startDepth - 10,
                        Symbol.Cross => 10 - startDepth,
                        _ => 0,
                    };
            }
        }

        if (isMaximizingPlayer)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Matrix.Instance[i, j] == Symbol.Empty)
                    {
                        //switch (SymbolStrategy)
                        //{
                        //    case CrossesStrategy:
                        //        Matrix.Instance[i, j] = Symbol.Cross;
                        //        break;
                        //    case NoughtsStrategy:
                        //        Matrix.Instance[i, j] = Symbol.Nought;
                        //        break;
                        //}

                        SymbolStrategy?.DrawSymbol(Field, i, j);

                        int score = MiniMax(startDepth + 1, false);

                        Matrix.Instance[i, j] = Symbol.Empty;

                        bestScore = Math.Max(score, bestScore);
                    }
                }
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Matrix.Instance[i, j] == Symbol.Empty)
                    {
                        //switch (Matrix.Instance.User?.Symbol)
                        //{
                        //    case Symbol.Cross:
                        //        Matrix.Instance[i, j] = Symbol.Cross;
                        //        break;
                        //    case Symbol.Nought:
                        //        Matrix.Instance[i, j] = Symbol.Nought;
                        //        break;
                        //}

                        SymbolStrategy?.DrawSymbol(Field, i, j);

                        int score = MiniMax(startDepth + 1, true);

                        Matrix.Instance[i, j] = Symbol.Empty;

                        bestScore = Math.Min(score, bestScore);
                    }
                }
            }

            return bestScore;
        }
    }
}
