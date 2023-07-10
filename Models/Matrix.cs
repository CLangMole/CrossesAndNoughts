using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models;

public class Matrix : IEnumerable<Symbol>
{
    public static Grid? Field { get; set; }
    public static Matrix Instance => _instance.Value;

    private static readonly Lazy<Matrix> _instance = new(() => new Matrix());
    private readonly Symbol[,] _state = new Symbol[3, 3];

    public Matrix()
    {
        IEnumerable<Image>? cellsWithSymbol = (Field?.Children?.OfType<Image>());
        IEnumerable<UIElement>? emptyCells = cellsWithSymbol is null ? Field?.Children.OfType<UIElement>() : Field?.Children.OfType<UIElement>().Except(cellsWithSymbol);

        if (emptyCells is null)
        {
            throw new NullReferenceException(nameof(emptyCells));
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                foreach (var cell in emptyCells)
                {
                    if ((int)cell.GetValue(Grid.RowProperty) == i && (int)cell.GetValue(Grid.ColumnProperty) == j)
                    {
                        _state[i, j] = Symbol.Empty;
                    }
                }
            }
        }
    }

    public IEnumerator<Symbol> GetEnumerator()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                yield return _state[i, j];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Symbol this[int row, int column]
    {
        get => _state[row, column];
        set => _state[row, column] = value;
    }
}
