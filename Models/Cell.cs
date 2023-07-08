using System.Windows.Controls;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CrossesAndNoughts.Models;

public class Cell
{
    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }

    public Image? Child { get; set; }

    private int _row;
    private int _column;

    public Cell(int row, int column)
    {
        _row = row;
        _column = column;
    }
}