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
    public User? CurrentUser { get; set; }
    public Opponent? CurrentOpponent { get; set; }

    public static Matrix Instance => _instance.Value;

    private static readonly Lazy<Matrix> _instance = new(() => new Matrix());
    private readonly Symbol[,] _state = new Symbol[3, 3];
    private readonly IEnumerable<Image>? _cellsWithSymbol;

    private static readonly Line[] _lines = new Line[]
    {
        new Line(new Position(0, 0), new Position(0, 1), new Position(0, 2)),
        new Line(new Position(1, 0), new Position(1, 1), new Position(1, 2)),
        new Line(new Position(2, 0), new Position(2, 1), new Position(2, 2)),

        new Line(new Position(0, 0), new Position(1, 0), new Position(2, 0)),
        new Line(new Position(0, 1), new Position(1, 1), new Position(2, 1)),
        new Line(new Position(0, 2), new Position(1, 2), new Position(2, 2)),

        new Line(new Position(0, 0), new Position(1, 1), new Position(2, 2)),
        new Line(new Position(0, 2), new Position(1, 1), new Position(2, 0))
    };

    public Matrix()
    {
        _cellsWithSymbol = (Field?.Children?.OfType<Image>());
        IEnumerable<UIElement>? emptyCells = _cellsWithSymbol is null ? Field?.Children.OfType<UIElement>() : Field?.Children.OfType<UIElement>().Except(_cellsWithSymbol);

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

    public GameStatus GetGameStatus()
    {
        int[] rowScores = new int[3];
        int[] columnScores = new int[3];
        int[] diag1Score = new int[1];
        int[] diag2Score = new int[1];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Symbol symbol = this[i, j];
                int delta = GetDelta(symbol);

                rowScores[i] += delta;
                columnScores[j] += delta;

                if (i == j)
                {
                    diag1Score[0] += delta;
                }

                if (i == 3 - j - 1)
                {
                    diag2Score[0] += delta;
                }
            }
        }

        foreach (Symbol symbol in new Symbol[] { Symbol.Cross, Symbol.Nought })
        {
            int winPoints = 3 * GetDelta(symbol);

            for (int i = 0; i < 3; i++)
            {
                if (rowScores[i] == winPoints || columnScores[i] == winPoints)
                {
                    return new GameStatus(true, symbol);
                }
            }

            if (diag1Score[0] == winPoints || diag2Score[0] == winPoints)
            {
                return new GameStatus(true, symbol);
            }
        }

        return new GameStatus(IsFieldFull(), Symbol.Empty);
    }

    private static int GetDelta(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.Cross => 1,
            Symbol.Nought => -1,
            Symbol.Empty => 0,
            _ => throw new NotImplementedException()
        }; 
    }

    public void ClearSymbol(int row, int column)
    {
        this[row, column] = Symbol.Empty;
    }

    public Matrix Copy()
    {
        Matrix matrix = new();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                matrix[i, j] = this[i, j];
            }
        }

        matrix.CurrentUser = CurrentUser;
        matrix.CurrentOpponent = CurrentOpponent;

        return matrix;
    }

    public void Reset()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _state[i, j] = Symbol.Empty;

                if (_cellsWithSymbol is null)
                {
                    return;
                }

                foreach (var symbol in _cellsWithSymbol)
                {
                    Field?.Children.Remove(symbol);
                }
            }
        }
    }

    private bool IsFieldFull()
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

    public static int Evaluate(Matrix matrix)
    {
        int score = 0;

        for (int i = 0; i < _lines.Length; i++)
        {
            score += EvaluateLine(_lines[i], matrix);
        }

        return score;
    }

    private static int EvaluateLine(Line line, Matrix matrix)
    {
        int score = 0;

        Symbol cell1 = matrix[line.GetCell(0).Row, line.GetCell(0).Column];
        Symbol cell2 = matrix[line.GetCell(1).Row, line.GetCell(1).Column];
        Symbol cell3 = matrix[line.GetCell(2).Row, line.GetCell(2).Column];

        if (cell1 == matrix.CurrentUser?.CurrentSymbol)
        {
            score = 1;
        }
        else if (cell1 == matrix.CurrentOpponent?.CurrentSymbol)
        {
            score = -1;
        }

        if (cell2 == matrix.CurrentUser?.CurrentSymbol)
        {
            if (score == 1)
            {
                score = 10;
            }
            else if (score == -1)
            {
                return 0;
            }
            else
            {
                score = 1;
            }
        }
        else if (cell2 == matrix.CurrentOpponent?.CurrentSymbol)
        {
            if (score == -1)
            {
                score = -10;
            }
            else if (score == 1)
            {
                return 0;
            }
            else
            {
                score = -1;
            }
        }

        if (cell3 == matrix.CurrentUser?.CurrentSymbol)
        {
            if (score > 0)
            {
                score *= 10;
            }
            else if (score < 0)
            {
                return 0;
            }
            else
            {
                score = 1;
            }
        }
        else if (cell3 == matrix.CurrentOpponent?.CurrentSymbol)
        {
            if (score < 0)
            {
                score *= 10;
            }
            else if (score > 0)
            {
                return 0;
            }
            else
            {
                score = -1;
            }
        }

        return score;
    }

    private class Line
    {
        private static readonly Position[] _line = new Position[3];

        public Line(Position cell1, Position cell2, Position cell3)
        {
            _line[0] = cell1;
            _line[1] = cell2;
            _line[2] = cell3;
        }

        public Position GetCell(int index)
        {
            return _line[index];
        }
    }
}

public class GameStatus
{
    public bool IsGameOver => _isGameOver;

    public Symbol WinnerSymbol => _winnerSymbol;


    private readonly Symbol _winnerSymbol;
    private readonly bool _isGameOver;

    public GameStatus(bool isGameOver, Symbol winnerSymbol)
    {
        _isGameOver = isGameOver;
        _winnerSymbol = winnerSymbol;
    }
}

public class Position
{
    public int Row => _row;
    public int Column => _column;

    private readonly int _row;
    private readonly int _column;

    public Position(int row, int column)
    {
        _row = row;
        _column = column;
    }
}

public class Score
{
    public int BestScore { get => _bestScore; set => _bestScore = value; }
    public int BestRow { get => _bestRow; set => _bestRow = value; }
    public int BestColumn { get => _bestColumn; set => _bestColumn = value; }

    private int _bestScore;
    private int _bestRow;
    private int _bestColumn;


    public Score(int bestScore, int bestRow, int bestColumn)
    {
        _bestScore = bestScore;
        _bestRow = bestRow;
        _bestColumn = bestColumn;
    }
}