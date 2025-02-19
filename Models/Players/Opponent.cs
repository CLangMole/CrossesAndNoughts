﻿using CrossesAndNoughts.Models.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CrossesAndNoughts.Models.Players;

public class Opponent : Player
{
    public (Brush, string) CurrentDifficulty { get; private set; } = (Brushes.YellowGreen, "Easy");
    public Symbol CurrentSymbol => _symbol;

    private readonly Symbol _symbol;
    private int _winsCount = 0;
    private int _difficulty = 1;

    public Opponent(ISymbolStrategy symbolStrategy) : base(symbolStrategy)
    {
        _symbol = symbolStrategy switch
        {
            CrossesStrategy => Symbol.Cross,
            NoughtsStrategy => Symbol.Nought,
            _ => throw new NotImplementedException()
        };
    }

    public override async Task Draw(int row, int column)
    {
        if (SymbolStrategy is null)
        {
            throw new NullReferenceException(nameof(SymbolStrategy));
        }

        if (Buttons is null || !Buttons.Any())
        {
            throw new("There're no buttons on the field");
        }

        await Task.Yield();

        SetButtonsActive(false);

        await Task.Delay(1000);

        var gameStatus = Matrix.Instance.GetGameStatus();

        if (gameStatus.IsGameOver)
        {
            if (gameStatus.WinnerSymbol != _symbol && gameStatus.WinnerSymbol != Symbol.Empty)
            {
                _winsCount += 2;
                SoundsControl.WinSound.Play();
                Matrix.DrawWinningLine();
                await Task.Delay(1000);
            }
            else if (gameStatus.WinnerSymbol == Symbol.Empty)
            {
                _winsCount++;
                SoundsControl.WinSound.Play();
            }
            else
            {
                Matrix.DrawWinningLine();
                SetButtonsActive(false);
                await Task.Delay(1000);
                SetButtonsActive(true);
            }

            if (_difficulty < 4)
            {
                _difficulty++;
            }

            CurrentDifficulty = _difficulty switch
            {
                0 => (Brushes.AliceBlue, "TooEasy"),
                1 => (Brushes.GreenYellow, "Easy"),
                2 => (Brushes.Yellow, "Middle"),
                3 => (Brushes.DarkOrange, "Hard"),
                4 => (Brushes.Red, "Insane"),
                _ => throw new NotImplementedException()
            };

            GameOver?.Invoke(_winsCount);

            Matrix.Reset();
            SetButtonsActive(true);

            return;
        }

        if (Matrix.Instance.CurrentUser?.CurrentSymbol is null || Matrix.Instance.CurrentOpponent?.CurrentSymbol is null)
        {
            throw new NullReferenceException("You didn't choose your symbol");
        }

        if (row == -1 && column == -1)
        {
            var miniMax = MiniMax(Matrix.Instance, _difficulty, _symbol, int.MinValue, int.MaxValue);

            row = miniMax.BestRow;
            column = miniMax.BestColumn;
        }

        SymbolStrategy?.DrawSymbol(Field, row, column);

        SetButtonsActive(true);

        gameStatus = Matrix.Instance.GetGameStatus();

        if (gameStatus.IsGameOver)
        {
            if (gameStatus.WinnerSymbol != Symbol.Empty)
            {
                Matrix.DrawWinningLine();
                await Task.Delay(1000);
            };

            GameOver?.Invoke(_winsCount);

            Matrix.Reset();
        }
    }

    private Score MiniMax(Matrix matrix, int depth, Symbol symbol, int alpha, int beta)
    {
        if (matrix.CurrentUser is null || matrix.CurrentOpponent is null)
        {
            throw new NullReferenceException();
        }

        int bestScore = (symbol == matrix.CurrentUser.CurrentSymbol) ? int.MinValue : int.MaxValue;
        int bestRow = -1;
        int bestColumn = -1;

        if (depth == 0 || matrix.GetGameStatus().IsGameOver)
        {
            bestScore = Matrix.Evaluate(matrix);
        }
        else
        {
            Matrix matrixClone = matrix.Copy();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i, j] == Symbol.Empty)
                    {
                        matrixClone[i, j] = symbol;

                        if (symbol == matrix.CurrentUser.CurrentSymbol)
                        {
                            int currentScore = MiniMax(matrixClone, depth - 1, matrix.CurrentOpponent.CurrentSymbol, alpha, beta).BestScore;

                            if (currentScore > bestScore)
                            {
                                bestScore = currentScore;
                                bestRow = i;
                                bestColumn = j;
                            }

                            alpha = Math.Max(alpha, bestScore);

                            if (beta <= alpha)
                            {
                                break;
                            }
                        }
                        else
                        {
                            int currentScore = MiniMax(matrixClone, depth - 1, matrix.CurrentUser.CurrentSymbol, alpha, beta).BestScore;

                            if (currentScore < bestScore)
                            {
                                bestScore = currentScore;
                                bestRow = i;
                                bestColumn = j;
                            }

                            beta = Math.Min(beta, bestScore);

                            if (beta <= alpha)
                            {
                                break;
                            }
                        }

                        matrixClone.ClearSymbol(i, j);
                    }
                }
            }
        }

        return new Score(bestScore, bestRow, bestColumn);
    }
}
