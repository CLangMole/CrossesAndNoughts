using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Players;

public abstract class Player(ISymbolStrategy symbolStrategy, Grid field)
{
    protected readonly IEnumerable<Button>? Buttons = field.Children.OfType<Button>();
    protected ISymbolStrategy SymbolStrategy { get; } = symbolStrategy;

    public static Action<int>? GameOver { get; set; }
    public Symbol CurrentSymbol => SymbolStrategy.PlayerSymbol;

    public virtual async Task Draw(int row, int column)
    {
        await Task.Yield();
    }

    protected void SetButtonsActive(bool isEnabled)
    {
        if (Buttons is null || !Buttons.Any())
        {
            throw new Exception("Buttons array is null or empty");
        }

        foreach (var button in Buttons)
        {
            button.IsEnabled = isEnabled;
        }
    }
}
