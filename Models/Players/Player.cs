using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models.Players;

public abstract class Player
{
    protected readonly IEnumerable<Button>? Buttons = Field?.Children.OfType<Button>();
    protected ISymbolStrategy? SymbolStrategy { get => _symbolStrategy; }

    public static Grid? Field { get; set; }

    public static Action<int>? GameOver;

    private readonly ISymbolStrategy _symbolStrategy;

    public Player(ISymbolStrategy symbolStrategy)
    {
        _symbolStrategy = symbolStrategy;
    }

    public virtual async Task Draw(int row, int column)
    {
        await Task.Yield();
    }

    public virtual void SetButtonsActive(bool isEnabled)
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
}
