using CrossesAndNoughts.Models.Strategies;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CrossesAndNoughts;

public abstract class Player
{
    public ISymbolStrategy? SymbolStrategy { get => _symbolStrategy; }

    public static Grid? Field { get; set; }

    private readonly ISymbolStrategy _symbolStrategy;

    public Player(ISymbolStrategy symbolStrategy)
    {
        _symbolStrategy = symbolStrategy;
    }

    public virtual void Draw()
    {
        _symbolStrategy.DrawSymbol(null, 0, 0);
    }
}

public class User : Player
{
    public User(ISymbolStrategy symbolStrategy) : base(symbolStrategy) { }

    public override void Draw()
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (Field is null)
        {
            throw new NullReferenceException(nameof(Field));
        }

        foreach (UIElement cell in Field.Children)
        {
            cell.MouseDown += (sender, e) =>
            {
                SymbolStrategy.DrawSymbol(Field, (int)cell.GetValue(Grid.RowProperty), (int)cell.GetValue(Grid.ColumnProperty));
            };
        }
    }
}

public class Opponent : Player
{
    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy) { }
}
