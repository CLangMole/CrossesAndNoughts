namespace CrossesAndNoughts.Models.Field;

public class Position(int row, int column)
{
    public int Row { get; } = row;

    public int Column { get; } = column;

    public override string ToString()
    {
        return $"({Row}, {Column})";
    }
}