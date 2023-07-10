using CrossesAndNoughts.Models.Strategies;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    public User(ISymbolStrategy symbolStrategy) : base(symbolStrategy) { }

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

    private readonly Random _random = new();

    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy) { }

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

        row = BestCell().Item1;
        column = BestCell().Item2;

        SymbolStrategy.DrawSymbol(Field, row, column);

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

    private Tuple<int, int> BestCell()
    {
        int row = 0;
        int column = 0;

        Queue<Tuple<int, int>> queue = new();
        queue.Enqueue(Tuple.Create(1, 1));

        while (queue.Count != 0)
        {
            var cell = queue.Dequeue();

            if (Matrix.Instance[cell.Item1, cell.Item2] != Symbol.Empty)
            {
                continue;
            }

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i != 0 && j != 0)
                    {
                        continue;
                    }
                    else
                    {
                        queue.Enqueue(Tuple.Create(row = cell.Item1 + i, column = cell.Item2 + j));
                    }
                }
            }
        }

        return Tuple.Create(row, column);
    }
}
