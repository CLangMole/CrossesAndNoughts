using CrossesAndNoughts.Models;
using CrossesAndNoughts.Models.DataBase;
using CrossesAndNoughts.Models.Players;
using CrossesAndNoughts.Models.Strategies;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CrossesAndNoughts.Models.Field;

namespace CrossesAndNoughts.ViewModel;

public class AppViewModel : INotifyPropertyChanged
{
    #region Commands

    public DelegateCommand GoNextCommand { get; } = new(ClickMethods.GoNext);
    public DelegateCommand GoBackCommand { get; } = new(ClickMethods.GoBack);
    public DelegateCommand QuitCommand { get; } = new(ClickMethods.Quit);
    public DelegateCommand GoBackToMenuCommand { get; } = new(GoBackToMenu);

    public DelegateCommand StartGameCommand { get; }
    public DelegateCommand SelectSymbolCommand { get; }
    private DelegateCommand DrawSymbolCommand { get; }
    public DelegateCommand SetupGridCommand { get; }

    #endregion

    #region Symbols

    public Symbol Cross
    {
        get => Symbol.Cross;
        private set
        {
            if (value != Symbol.Empty)
            {
                NotifyPropertyChanged(nameof(Cross));
            }
        }
    }

    public Symbol Nought
    {
        get => Symbol.Nought;
        private set
        {
            if (value != Symbol.Empty)
            {
                NotifyPropertyChanged(nameof(Nought));
            }
        }
    }

    #endregion

    #region Properties

    public string Points
    {
        get => _gameResult.ToString();
        set
        {
            _gameResult = int.Parse(value);
            NotifyPropertyChanged(nameof(Points));
        }
    }

    public System.Windows.Media.Brush DifficultyColor
    {
        get => _difficultyColor;
        set
        {
            _difficultyColor = value;
            NotifyPropertyChanged(nameof(DifficultyColor));
        }
    }

    public string DifficultyName
    {
        get => _difficultyName;
        set
        {
            _difficultyName = value;
            NotifyPropertyChanged(nameof(DifficultyName));
        }
    }

    public List<UserRecord> Records
    {
        get => _records;
        private set
        {
            _records = value;
            NotifyPropertyChanged(nameof(Records));
        }
    }

    #endregion

    #region Fields

    public event PropertyChangedEventHandler? PropertyChanged;

    private List<UserRecord> _records = [];

    private readonly StartWindow _startWindow;
    private readonly GameWindow _gameWindow;

    private User _user = null!;
    private Opponent _opponent = null!;

    private ISymbolStrategy _userSymbolStrategy = null!;
    private ISymbolStrategy _opponentSymbolStrategy = null!;

    private readonly Dictionary<Symbol, Func<ISymbolStrategy>> _strategyMap = new()
    {
        { Symbol.Cross, () => new CrossesStrategy() },
        { Symbol.Nought, () => new NoughtsStrategy() }
    };

    private int _gameResult;

    private System.Windows.Media.Brush _difficultyColor = System.Windows.Media.Brushes.GreenYellow;
    private string _difficultyName = "Easy";

    private string _userName = " ";
    private int _fieldSize = 3;

    private Matrix _matrix = null!;

    #endregion

    public AppViewModel(StartWindow startWindow, GameWindow gameWindow)
    {
        _startWindow = startWindow;
        _gameWindow = gameWindow;

        _startWindow.DataContext = this;
        _gameWindow.DataContext = this;

        SoundsControl.StartSound.Play();

        StartGameCommand = new DelegateCommand(StartGame);
        SelectSymbolCommand = new DelegateCommand(SelectSymbol);
        DrawSymbolCommand = new DelegateCommand(DrawSymbol);
        SetupGridCommand = new DelegateCommand(SetupGrid);

        using var recordsProxy = new UserRecordsProxy();
        Records = recordsProxy.GetRecords();
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void StartGame(object? parameter)
    {
        if (_startWindow.LoginLabel.Content is not Grid loginGrid)
        {
            throw new Exception($"Cannot find element {nameof(loginGrid)}");
        }

        if (((Viewbox)loginGrid.Children[1]).Child is not TextBox loginField)
        {
            throw new Exception($"Cannot find element {nameof(loginField)}");
        }

        if (string.IsNullOrEmpty(loginField.Text))
        {
            _userName = "Player" + new Random().Next(1, 1000000);
        }
        else
        {
            _userName = loginField.Text;
        }

        _startWindow.Hide();
        ClickMethods.GoBack(_startWindow.LoginLabel);
        SoundsControl.StartSound.Stop();

        _gameWindow.Height = _startWindow.ActualHeight;
        _gameWindow.Width = _startWindow.ActualWidth;
        _gameWindow.Left = _startWindow.Left;
        _gameWindow.Top = _startWindow.Top;
        _gameWindow.WindowState = _startWindow.WindowState;

        _gameWindow.Show();

        SoundsControl.GameSound.Play();
    }

    private void SelectSymbol(object? parameter)
    {
        if (parameter is not Symbol symbol)
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        var symbols = new[] { Symbol.Cross, Symbol.Nought };

        _userSymbolStrategy = _strategyMap[symbol].Invoke();
        _opponentSymbolStrategy = _strategyMap[symbols.Single(x => x != symbol)].Invoke();

        ClickMethods.GoNext(_gameWindow.SelectFieldSizeLabel);
    }

    private void SetupGrid(object? parameter)
    { 
        if (parameter is int fieldSize)
        {
            _fieldSize = fieldSize;
        }

        var gameField = new Grid { Margin = new Thickness(50), MaxHeight = 800, MaxWidth = 1000 };

        var columnDefinitions = new ColumnDefinition[_fieldSize];
        var rowDefinitions = new RowDefinition[_fieldSize];

        for (var i = 0; i < _fieldSize; i++)
        {
            columnDefinitions[i] = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };

            gameField.ColumnDefinitions.Add(columnDefinitions[i]);

            rowDefinitions[i] = new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            };

            gameField.RowDefinitions.Add(rowDefinitions[i]);
        }

        var borders = new Border[_fieldSize, _fieldSize];
        var buttons = new Button[_fieldSize, _fieldSize];

        for (var i = 0; i < _fieldSize; i++)
        {
            for (var j = 0; j < _fieldSize; j++)
            {
                borders[i, j] = new Border
                {
                    BorderBrush = System.Windows.Media.Brushes.DarkSlateBlue,
                    BorderThickness = new Thickness(0, 0, i == _fieldSize - 1 ? 0 : 2, j == _fieldSize - 1 ? 0 : 2)
                };

                borders[i, j].SetValue(Grid.ColumnProperty, i);
                borders[i, j].SetValue(Grid.RowProperty, j);
                gameField.Children.Add(borders[i, j]);
            }
        }

        for (var i = 0; i < _fieldSize; i++)
        {
            for (var j = 0; j < _fieldSize; j++)
            {
                buttons[i, j] = new Button
                {
                    Opacity = 0,
                    Command = DrawSymbolCommand,
                    CommandParameter = new Position(i, j)
                };

                buttons[i, j].SetValue(Grid.ColumnProperty, j);
                buttons[i, j].SetValue(Grid.RowProperty, i);
                gameField.Children.Add(buttons[i, j]);
            }
        }

        _gameWindow.GameUiContainer.Children.Add(gameField);
        
        ClickMethods.GoNext(_gameWindow.GameUiContainer);

        _matrix = new Matrix(_fieldSize, gameField);

        _user = new User(_userSymbolStrategy, _matrix);
        _opponent = new Opponent(_opponentSymbolStrategy, _matrix);

        _matrix.SetPlayersSymbols(_user.CurrentSymbol, _opponent.CurrentSymbol);

        if (_opponent.CurrentSymbol == Symbol.Cross)
        {
            _ = _opponent.Draw(-1, -1);
        }

        _user.UserDrewSymbol += () =>
        {
            var draw = _opponent.Draw(-1, -1);
            draw.Start();
        };

        Player.GameOver += pointsCount =>
        {
            if (pointsCount <= _gameResult)
            {
                var record = _gameResult;
                SetGameOver();

                using var records = new UserRecordsProxy();
                records.AddRecord(new UserRecord(_userName, records.GetRecords().Count + 1, record));
                Records = records.GetRecords();
            }

            Points = pointsCount.ToString();
            DifficultyColor = _opponent.CurrentDifficulty.Item1;
            DifficultyName = _opponent.CurrentDifficulty.Item2;
        };
    }

    private void DrawSymbol(object? parameter)
    {
        if (parameter is not Position position)
        {
            throw new ArgumentException(null, nameof(parameter));
        }
        _ = _user.Draw(position.Row, position.Column);
    }

    private void SetGameOver()
    {
        SoundsControl.GameSound.Stop();
        SoundsControl.GameOverSound.Play();

        SoundsControl.GameOverSound.MediaEnded += (_, _) => { ClickMethods.GoNext(_gameWindow.GameOverLabel); };
    }

    private static void GoBackToMenu(object? parameter)
    {
        if (Environment.ProcessPath is null)
        {
            return;
        }

        Process.Start(Environment.ProcessPath);
        Application.Current.Shutdown();
    }
}