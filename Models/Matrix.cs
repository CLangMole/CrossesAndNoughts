using CrossesAndNoughts.Models.Players;
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
    public User? User { get; set; }
    public Opponent? Opponent { get; set; }

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

    public Symbol Winner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (this[i, 0] != Symbol.Empty && this[i, 0] == this[i, 1] && this[i, 1] == this[i, 2])
            {
                return this[i, 0];
            }

            if (this[0, i] != Symbol.Empty && this[0, i] == this[1, i] && this[1, i] == this[2, i])
            {
                return this[0, i];
            }

            if (this[0, 0] != Symbol.Empty && this[0, 0] == this[1, 1] && this[1, 1] == this[2, 2])
            {
                return this[0, 0];
            }

            if (this[0, 2] != Symbol.Empty && this[0, 2] == this[1, 1] && this[1, 1] == this[2, 0])
            {
                return this[0, 2];
            }
        }

        return Symbol.Empty;
    }

    public bool IsFieldFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this[i, j] == Symbol.Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsGameOver()
    {
        return IsFieldFull() || Winner() != Symbol.Empty;
    }

    public void ClearSymbol(int row, int column)
    {
        IEnumerable<Image>? symbols = (Field?.Children.OfType<Image>()) ?? throw new NullReferenceException(nameof(Field));

        Field?.Children.Remove(symbols.Single(x => (int)x.GetValue(Grid.RowProperty) == row && (int)x.GetValue(Grid.ColumnProperty) == column));

        this[row, column] = Symbol.Empty;
    }
}
