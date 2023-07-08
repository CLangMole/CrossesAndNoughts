using System.Windows.Controls;

namespace CrossesAndNoughts.Models;

public class Cell
{
    public int Row { get => _row; set => _row = value; }
    public int Column { get => _column; set => _column = value; }

    public Image? Child { get => _child; set => _child = value; }

    private int _row;
    private int _column;

    private Image? _child;

    public Cell(int row, int column)
    {
        _row = row;
        _column = column;
    }

    public void AddItem(Image child)
    {
        _child = child;
    }
}