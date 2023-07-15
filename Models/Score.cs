namespace CrossesAndNoughts.Models;

public class Score
{
    public int BestScore => _bestScore;
    public int BestRow => _bestRow;
    public int BestColumn => _bestColumn;

    private readonly int _bestScore;
    private readonly int _bestRow;
    private readonly int _bestColumn;

    public Score(int bestScore, int bestRow, int bestColumn)
    {
        _bestScore = bestScore;
        _bestRow = bestRow;
        _bestColumn = bestColumn;
    }
}
