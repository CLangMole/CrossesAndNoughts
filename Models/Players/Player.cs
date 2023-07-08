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

        if (Buttons is null || !Buttons.Any())
        {
            throw new Exception("There's no button on the field");
        }

        SymbolStrategy.DrawSymbol(Field, row, column);

        foreach (Button? button in Buttons)
        {
            button.IsEnabled = false;
        }

        await Task.Delay(1000);

        UserDrawedSymbol?.Invoke();
    }
}

public class Opponent : Player
{
    public event Action? OpponentDrawedSymbol;

    private readonly Random _random = new();

    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy) { }

    public override void Draw(int row, int column)
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

        row = BestWay().Item1;
        column = BestWay().Item2;

        SymbolStrategy.DrawSymbol(Field, row, column);

        SetButtonActive(true);

        OpponentDrawedSymbol?.Invoke();
    }

    private Tuple<int, int> BestWay()
    {
        int row = 0;
        int column = 0;

        IEnumerable<Cell> cellsWithSymbol = Matrix.Instance.Where(x => x.Child != null);

        //if (!cellsWithSymbol.Any())
        //{
        //    throw new Exception($"There're no symbols on the field. Count : {cellsWithSymbol.Count()}");
        //}

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //foreach (Cell cell in cellsWithSymbol)
                //{
                //    if (cell.Row == i || cell.Column == j)
                //    {
                //        continue;
                //    }

                //    row = _random.Next(0, i);
                //    column = _random.Next(0, j);
                //}

                IEnumerable<UIElement>? symbols = (Field?.Children.OfType<Image>()) ?? throw new NullReferenceException();

                foreach (UIElement symbol in symbols)
                {
                    if (i == (int)symbol.GetValue(Grid.RowProperty) && j == (int)symbol.GetValue(Grid.ColumnProperty))
                    {
                        continue;
                    }

                    int randomRow = _random.Next(0, i);
                    int randomColumn = _random.Next(0, j);

                    if (randomRow == (int)symbol.GetValue(Grid.RowProperty) && randomColumn == (int)symbol.GetValue(Grid.ColumnProperty))
                    {
                        continue;
                    }

                    row = randomRow;
                    column = randomColumn;
                }
            }
        }

        return Tuple.Create(row, column);
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
}
