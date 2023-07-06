using CrossesAndNoughts.Models.Strategies;
using CrossesAndNoughts.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CrossesAndNoughts.Models.Players;

public abstract class Player
{
    public ISymbolStrategy? SymbolStrategy { get => _symbolStrategy; }

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

        SymbolStrategy.DrawSymbol(Field, row, column);

        UserDrawedSymbol?.Invoke();
    }
}

public class Opponent : Player
{
    public event Action? OpponentDrawedSymbol;

    private readonly Random _random = new();
    private readonly IEnumerable<Button>? _emptyCells = Field?.Children.OfType<Button>().Where(c => c.IsEnabled == true);
    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy)
    {
        _emptyCells = Field?.Children.OfType<Button>().Where(c => c.IsEnabled == true);
    }

    public override void Draw(int row, int column)
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (_emptyCells is null)
        {
            throw new NullReferenceException(nameof(_emptyCells));
        }

        int rowsCount = 0;
        int columnsCount = 0;

        foreach (Button cell in _emptyCells)
        {
            rowsCount++;
            columnsCount++;

            row = _random.Next(0, rowsCount);
            column = _random.Next(0, columnsCount);

            if ((int)cell.GetValue(Grid.RowProperty) == row && (int)cell.GetValue(Grid.ColumnProperty) == column)
            {
                cell.IsEnabled = false;
            }
        }

        SymbolStrategy.DrawSymbol(Field, row, column);

        OpponentDrawedSymbol?.Invoke();
    }
}
