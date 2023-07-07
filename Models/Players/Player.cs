using CrossesAndNoughts.Models.Strategies;
using CrossesAndNoughts.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CrossesAndNoughts.Models.Players;

public abstract class Player
{
    protected readonly IEnumerable<Button>? buttons = Field?.Children.OfType<Button>();
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

    public override async void Draw(int row, int column)
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (Field is null)
        {
            throw new NullReferenceException(nameof(Field));
        }

        if (buttons is null || !buttons.Any())
        {
            throw new Exception("There's no button on the field");
        }

        SymbolStrategy.DrawSymbol(Field, row, column);

        await Task.Delay(1000);

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

        if (buttons is null || !buttons.Any())
        {
            throw new Exception("There's no button on the field");
        }

        foreach (Button? button in buttons)
        {
            button.IsEnabled = false;
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                IEnumerable<UIElement>? symbols = Field?.Children.OfType<Image>();

                if (symbols is not null && symbols.Any())
                {
                    foreach (UIElement? symbol in symbols)
                    {
                        if ((int)symbol.GetValue(Grid.RowProperty) == i || (int)symbol.GetValue(Grid.ColumnProperty) == j)
                        {
                            continue;
                        }

                        row = _random.Next(0, i);
                        column = _random.Next(0, j);
                    }
                }
                else
                {
                    row = _random.Next(0, i);
                    column = _random.Next(0, j);
                }
            }
        }

        SymbolStrategy.DrawSymbol(Field, row, column);

        OpponentDrawedSymbol?.Invoke();

        await Task.Delay(1000);

        foreach (Button? button in buttons)
        {
            button.IsEnabled = true;
        }
    }
}
