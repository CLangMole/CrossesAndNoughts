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

    private static readonly Line[] _lines = new Line[]
    {
        new Line(Tuple.Create(0, 0), Tuple.Create(0, 1), Tuple.Create(0, 2)),
        new Line(Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2)),
        new Line(Tuple.Create(2, 0), Tuple.Create(2, 1), Tuple.Create(2, 2)),

        new Line(Tuple.Create(0, 0), Tuple.Create(1, 0), Tuple.Create(2, 0)),
        new Line(Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(2, 1)),
        new Line(Tuple.Create(0, 2), Tuple.Create(1, 2), Tuple.Create(2, 2)),

        new Line(Tuple.Create(0, 0), Tuple.Create(1, 1), Tuple.Create(2, 2)),
        new Line(Tuple.Create(0, 2), Tuple.Create(1, 1), Tuple.Create(2, 0))
    };

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

    //public Symbol Winner()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        if (this[i, 0] != Symbol.Empty && this[i, 0] == this[i, 1] && this[i, 1] == this[i, 2])
    //        {
    //            return this[i, 0];
    //        }

    //        if (this[0, i] != Symbol.Empty && this[0, i] == this[1, i] && this[1, i] == this[2, i])
    //        {
    //            return this[0, i];
    //        }

    //        if (this[0, 0] != Symbol.Empty && this[0, 0] == this[1, 1] && this[1, 1] == this[2, 2])
    //        {
    //            return this[0, 0];
    //        }

    //        if (this[0, 2] != Symbol.Empty && this[0, 2] == this[1, 1] && this[1, 1] == this[2, 0])
    //        {
    //            return this[0, 2];
    //        }
    //    }

    //    return Symbol.Empty;
    //}

    public Tuple<bool, Symbol> GetGameStatus()
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
                    return Tuple.Create(true, symbol);
                }
            }

            if (diag1Score[0] == winPoints || diag2Score[0] == winPoints)
            {
                return Tuple.Create(true, symbol);
            }
        }

        return Tuple.Create(IsFieldFull(), Symbol.Empty);
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
        IEnumerable<Image>? symbols = (Field?.Children.OfType<Image>()) ?? throw new NullReferenceException(nameof(Field));

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

        return matrix;
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

    public int Evaluate()
    {
        int score = 0;

        foreach (var line in _lines)
        {
            score += EvaluateLine(line);
        }

        return score;
    }

    private int EvaluateLine(Line line)
    {
        int score = 0;

        Symbol cell1 = this[line.GetCell(0).Item1, line.GetCell(0).Item2];
        Symbol cell2 = this[line.GetCell(1).Item1, line.GetCell(1).Item2];
        Symbol cell3 = this[line.GetCell(2).Item1, line.GetCell(2).Item2];

        if (cell1 == CurrentUser?.CurrentSymbol)
        {
            score = 1;
        }
        else if (cell1 == CurrentOpponent?.CurrentSymbol)
        {
            score = -1;
        }

        if (cell2 == CurrentUser?.CurrentSymbol)
        {
            switch (score)
            {
                case 1:
                    score = 10;
                    break;
                case -1:
                    return 0;
                default:
                    score = -1;
                    break;
            }
        }
        else if (cell2 == CurrentOpponent?.CurrentSymbol)
        {
            switch (score)
            {
                case -1:
                    score = -10;
                    break;
                case 1:
                    return 0;
                default:
                    score = -1;
                    break;
            }
        }

        if (cell3 == CurrentUser?.CurrentSymbol)
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
        else if (cell3 == CurrentOpponent?.CurrentSymbol)
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
        private static readonly Tuple<int, int>[] _line = new Tuple<int, int>[3];

        public Line(Tuple<int, int> cell1, Tuple<int, int> cell2, Tuple<int, int> cell3)
        {
            _line[0] = cell1;
            _line[1] = cell2;
            _line[2] = cell3;
        }

        public Tuple<int, int> GetCell(int index)
        {
            return _line[index];
        }
    }
}
