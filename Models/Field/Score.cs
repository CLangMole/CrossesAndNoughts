namespace CrossesAndNoughts.Models.Field;

public class Score(int bestScore, int bestRow, int bestColumn)
{
    public int BestScore { get; } = bestScore;

    public int BestRow { get; } = bestRow;

    public int BestColumn { get; } = bestColumn;
}