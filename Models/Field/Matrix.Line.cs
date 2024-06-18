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
        public LineType Type { get; }

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
                                  || Math.Abs(grid.ActualHeight - grid.MaxHeight) < float.Epsilon ? 20 : 10
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

            var lineFirstCell = line.GetCell(0);
            var lineLastCell = line.GetCell(line.GetCount() - 1);

            var cellSize = grid.ActualWidth / 3;
            var margin = cellSize / 2;

            Point from, to;

            foreach (var cell in cellsWithSymbol)
            {
                var row = (int)cell.GetValue(Grid.RowProperty);
                var column = (int)cell.GetValue(Grid.ColumnProperty);

                switch (line.Type)
                {
                    case LineType.Column when row == lineFirstCell.Row
                                              && column == lineFirstCell.Column:
                        from = row switch
                        {
                            0 => new Point(0, margin - 15),
                            1 => new Point(0, grid.ActualHeight / 2),
                            2 => new Point(0, grid.ActualHeight - margin + 15),
                            _ => throw new NotImplementedException()
                        };

                        continue;
                    case LineType.Column when row == lineLastCell.Row
                                              && column == lineLastCell.Column:
                        to = row switch
                        {
                            0 => new Point(grid.ActualWidth, margin - 15),
                            1 => new Point(grid.ActualWidth, grid.ActualHeight / 2),
                            2 => new Point(grid.ActualWidth, grid.ActualHeight - margin + 15),
                            _ => throw new NotImplementedException()
                        };

                        continue;
                    case LineType.Row when row == lineFirstCell.Row
                                           && column == lineFirstCell.Column:
                        from = column switch
                        {
                            0 => new Point(margin, 0),
                            1 => new Point(grid.ActualWidth / 2, 0),
                            2 => new Point(grid.ActualWidth - margin, 0),
                            _ => throw new NotImplementedException()
                        };

                        continue;
                    case LineType.Row when row == lineLastCell.Row
                                           && column == lineLastCell.Column:
                        to = column switch
                        {
                            0 => new Point(margin, grid.ActualHeight),
                            1 => new Point(grid.ActualWidth / 2, grid.ActualHeight),
                            2 => new Point(grid.ActualWidth - margin, grid.ActualHeight),
                            _ => throw new NotImplementedException()
                        };

                        continue;
                    case LineType.Diagonal when row == lineFirstCell.Row
                                                && column == lineFirstCell.Column:
                        from = column switch
                        {
                            0 => new Point(0, 0),
                            2 => new Point(grid.ActualWidth, 0),
                            _ => throw new NotImplementedException()
                        };

                        continue;
                    case LineType.Diagonal when row == lineLastCell.Row
                                                && column == lineLastCell.Column:
                        to = column switch
                        {
                            0 => new Point(0, grid.ActualHeight),
                            2 => new Point(grid.ActualWidth, grid.ActualHeight),
                            _ => throw new NotImplementedException()
                        };

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
        };
    }
}