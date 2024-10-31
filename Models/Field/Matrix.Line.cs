using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CrossesAndNoughts.Models.Field;

public partial class Matrix
{
    private class Line
    {
        private LineType Type { get; }

        private readonly List<Position> _line = [];

        internal Line(LineType lineType, params Position[] cells)
        {
            _line.AddRange(cells);

            Type = lineType;
        }

        internal Position GetCell(int index)
        {
            return _line[index];
        }

        private int GetCount()
        {
            return _line.Count;
        }

        internal static void Visualize(Point from, Point to, Grid grid)
        {
            DoubleAnimation lineXAnimation = new()
            {
                From = from.X,
                To = to.X,
                Duration = TimeSpan.FromMilliseconds(500)
            };

            DoubleAnimation lineYAnimation = new()
            {
                From = from.Y,
                To = to.Y,
                Duration = TimeSpan.FromMilliseconds(500)
            };

            System.Windows.Shapes.Line visualLine = new()
            {
                X1 = from.X,
                Y1 = from.Y,

                Fill = System.Windows.Media.Brushes.Violet,
                Visibility = Visibility.Visible,
                Stroke = System.Windows.Media.Brushes.Violet,

                StrokeThickness = Math.Abs(grid.ActualWidth - grid.MaxWidth) < float.Epsilon
                                  || Math.Abs(grid.ActualHeight - grid.MaxHeight) < float.Epsilon
                    ? 20
                    : 10
            };

            grid.Children.Add(visualLine);

            Grid.SetRowSpan(visualLine, grid.RowDefinitions.Count);
            Grid.SetColumnSpan(visualLine, grid.ColumnDefinitions.Count);

            visualLine.BeginAnimation(System.Windows.Shapes.Line.X2Property, lineXAnimation);
            visualLine.BeginAnimation(System.Windows.Shapes.Line.Y2Property, lineYAnimation);
        }

        internal static (Point from, Point to) GetPosition(Line line, Grid grid)
        {
            var cellsWithSymbol = grid.Children.OfType<Image>();

            var cellsCount = line.GetCount();

            var lineFirstCell = line.GetCell(0);
            var lineLastCell = line.GetCell(cellsCount - 1);

            var cellHeight = grid.ActualHeight / line.GetCount();
            var rowMargin = cellHeight / 2;

            var cellWidth = grid.ActualWidth / line.GetCount();
            var columnMargin = cellWidth / 2;


            Point from, to;

            foreach (var cell in cellsWithSymbol)
            {
                var row = (int)cell.GetValue(Grid.RowProperty);
                var column = (int)cell.GetValue(Grid.ColumnProperty);

                switch (line.Type)
                {
                    case LineType.Column when row == lineFirstCell.Row
                                              && column == lineFirstCell.Column:
                        if (row == 0)
                            from = new Point(column * cellWidth + columnMargin, 0);
                        else if (row == cellsCount - 1)
                            from = new Point(column * cellWidth + columnMargin, grid.ActualHeight);

                        continue;
                    case LineType.Column when row == lineLastCell.Row
                                              && column == lineLastCell.Column:
                        if (row == 0)
                            to = new Point(column * cellWidth + columnMargin, 0);
                        else if (row == cellsCount - 1)
                            to = new Point(column * cellWidth + columnMargin, grid.ActualHeight);

                        continue;
                    case LineType.Row when row == lineFirstCell.Row
                                           && column == lineFirstCell.Column:
                        if (column == 0)
                            from = new Point(0, row * cellHeight + rowMargin);
                        else if (column == cellsCount - 1)
                            from = new Point(grid.ActualWidth, row * cellHeight + rowMargin);

                        continue;
                    case LineType.Row when row == lineLastCell.Row
                                           && column == lineLastCell.Column:
                        if (column == 0)
                            to = new Point(0, row * cellHeight + rowMargin);
                        else if (column == cellsCount - 1)
                            to = new Point(grid.ActualWidth, row * cellHeight + rowMargin);

                        continue;
                    case LineType.Diagonal when row == lineFirstCell.Row
                                                && column == lineFirstCell.Column:
                        if (column == 0)
                            from = new Point(0, 0);
                        else if (column == cellsCount - 1)
                            from = new Point(grid.ActualWidth, 0);

                        continue;
                    case LineType.Diagonal when row == lineLastCell.Row
                                                && column == lineLastCell.Column:
                        if (column == 0)
                            to = new Point(0, grid.ActualHeight);
                        else if (column == cellsCount - 1)
                            to = new Point(grid.ActualWidth, grid.ActualHeight);

                        continue;
                    default:
                        continue;
                }
            }

            return (from, to);
        }

        internal enum LineType
        {
            Row,
            Column,
            Diagonal
        }
    }
}