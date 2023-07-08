using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CrossesAndNoughts.Models;

public class Matrix : IEnumerable<Cell>
{
    public static Matrix Instance => _instance.Value;

    private static readonly Lazy<Matrix> _instance = new(() => new  Matrix());

    private Cell[,] _cells => new Cell[3, 3]
            {
                { new Cell(0, 0), new Cell(0, 1),
                new Cell(0, 2) },

                { new Cell(1, 0),
                new Cell(1, 1),
                new Cell(1, 2) },

                { new Cell(2, 0),
                new Cell(2, 1),
                new Cell(2, 2) }
            };

    public Matrix() { }

    public IEnumerator<Cell> GetEnumerator()
    {
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                yield return _cells[i, j];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Cell this[int row, int column]
    {
        get => _cells[row, column];
    }

    public void AddItem(Image item, int row, int column)
    {
        _cells[row, column].Child = item;
    }

    public void RemoveItem(Image image, int row, int column)
    {
        _cells[row, column].Child = null;
    }
}
