using CrossesAndNoughts.Models.Strategies;
using CrossesAndNoughts.ViewModel.Commands;
using System;
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
    }
}

public class Opponent : Player
{
    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy) { }
}
