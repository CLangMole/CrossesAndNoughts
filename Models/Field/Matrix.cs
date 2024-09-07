using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CrossesAndNoughts.Models.Strategies;

namespace CrossesAndNoughts.Models.Field;

public partial class Matrix
{
    private const int MinSize = 3;
    
    public Symbol UserSymbol { get; private set; }
    public Symbol OpponentSymbol { get; private set; }

    public Grid Field { get; }

    public int Size { get; }

    private Symbol[,] _state;
    private readonly List<Line> _lines = [];

    public Matrix(int size, Grid field)
    {
        if (size < MinSize)
        {
            throw new ArgumentException("Invalid field size");
        }

        Size = size;
        
        Field = field;
        _state = new Symbol[size, size];
        
        List<Position> diagonal1Positions = [];
        List<Position> diagonal2Positions = [];
        
        for (var i = 0; i < size; i++)
        {
            List<Position> columnsPositions = [];
            List<Position> rowsPositions = [];
            
            diagonal1Positions = [];
            diagonal2Positions = [];
            
            for (var j = 0; j < size; j++)
            {
                _state[i, j] = Symbol.Empty;

                columnsPositions.Add(new Position(j, i));
                rowsPositions.Add(new Position(i, j));
                
                diagonal1Positions.Add(new Position(j, j));
                diagonal2Positions.Add(new Position(j, size - j - 1));
            }

            _lines.Add(new Line(Line.LineType.Column, columnsPositions.ToArray()));
            _lines.Add(new Line(Line.LineType.Row, rowsPositions.ToArray()));
        }
        
        _lines.Add(new Line(Line.LineType.Diagonal, diagonal1Positions.ToArray()));
        _lines.Add(new Line(Line.LineType.Diagonal, diagonal2Positions.ToArray()));
    }

    public Symbol this[int row, int column]
    {
        get => _state[row, column];
        set => _state[row, column] = value;
    }

    public void SetPlayersSymbols(Symbol userSymbol, Symbol opponentSymbol)
    {
        UserSymbol = userSymbol;
        OpponentSymbol = opponentSymbol;
    }
    
    public GameStatus GetGameStatus()
    {
        var rowScores = new int[Size];
        var columnScores = new int[Size];
        var diagonal1Score = new int[1];
        var diagonal2Score = new int[1];

        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                var symbol = this[i, j];
                var delta = GetDelta(symbol);

                rowScores[i] += delta;
                columnScores[j] += delta;

                if (i == j)
                {
                    diagonal1Score[0] += delta;
                }

                if (i == Size - j - 1)
                {
                    diagonal2Score[0] += delta;
                }
            }
        }

        foreach (var symbol in new[] { Symbol.Cross, Symbol.Nought })
        {
            var winPoints = Size * GetDelta(symbol);

            for (var i = 0; i < Size; i++)
            {
                if (rowScores[i] == winPoints || columnScores[i] == winPoints)
                {
                    return new GameStatus(true, symbol);
                }
            }

            if (diagonal1Score[0] == winPoints || diagonal2Score[0] == winPoints)
            {
                return new GameStatus(true, symbol);
            }
        }

        return new GameStatus(IsFieldFull(), Symbol.Empty);
    }


    public void ClearSymbol(int row, int column)
    {
        this[row, column] = Symbol.Empty;
    }

    public Matrix Copy()
    {
        Matrix matrix = new(Size, Field);

        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                matrix[i, j] = this[i, j];
            }
        }
        
        matrix.SetPlayersSymbols(UserSymbol, OpponentSymbol);
        
        return matrix;
    }

    public void Reset()
    {
        Field.Children.RemoveRange(2 * Size * Size, Size * Size + 1);

        foreach (var cell in Field.Children.OfType<Image>())
        {
            Field.Children.Remove(cell);
        }

        _state = new Symbol[Size, Size];
    }

    public void DrawWinningLine()
    {
        foreach (var line in _lines)
        {
            List<Symbol> cells = [];
            
            for (var i = 0; i < Size; i++)
            {
                cells.Add(this[line.GetCell(i).Row, line.GetCell(i).Column]);
            }
            
            if (cells.Any(x => x == Symbol.Empty))
            {
                continue;
            }

            var differentMiddleCells = from middleCell in cells
                from other in cells.Where(other => middleCell != other)
                select middleCell;

            if (differentMiddleCells.Any())
            {
                continue;
            }

            Line.Visualize(Line.GetPosition(line, Field).from, Line.GetPosition(line, Field).to, Field);
        }
    }

    public int Evaluate()
    {
        return _lines.Sum(l => EvaluateLine(l, this));
    }

    private int EvaluateLine(Line line, Matrix matrix)
    {
        var score = 0;

        List<Symbol> cells = [];
        
        for (var i = 0; i < Size; i++)
        {
            cells.Add(matrix[line.GetCell(i).Row, line.GetCell(i).Column]);
        }

        var firstCell = cells[0];
        var lastCell = cells[^1];
        
        if (firstCell == matrix.UserSymbol)
        {
            score = 1;
        }
        else if (firstCell == matrix.OpponentSymbol)
        {
            score = -1;
        }

        var iterations = 0;

        var middleCells = cells.GetRange(1, Size - 2);
        
        foreach (var middleCell in middleCells)
        {
            if (middleCell == matrix.UserSymbol)
            {
                switch (score)
                {
                    case 1:
                        score = iterations + 10;
                        break;
                    case -1:
                        return 0;
                    default:
                        score = 1;
                        break;
                }
            }
            else if (middleCell == matrix.OpponentSymbol)
            {
                switch (score)
                {
                    case -1:
                        score = -(iterations + 10);
                        break;
                    case 1:
                        return 0;
                    default:
                        score = -1;
                        break;
                }
            }
            
            iterations++;
        }

        if (lastCell == matrix.UserSymbol)
        {
            switch (score)
            {
                case > 0:
                    score *= 10;
                    break;
                case < 0:
                    return 0;
                default:
                    score = 1;
                    break;
            }
        }
        else if (lastCell == matrix.OpponentSymbol)
        {
            switch (score)
            {
                case < 0:
                    score *= 10;
                    break;
                case > 0:
                    return 0;
                default:
                    score = -1;
                    break;
            }
        }

        return score;
    }
    
    private bool IsFieldFull()
    {
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                if (this[i, j] == Symbol.Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static int GetDelta(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.Cross => 1,
            Symbol.Nought => -1,
            Symbol.Empty => 0,
            _ => throw new ArgumentException("Invalid symbol")
        };
    }
}