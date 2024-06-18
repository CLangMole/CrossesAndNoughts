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

public partial class AppViewModel : INotifyPropertyChanged
{
    #region Windows
    public static StartWindow StartWindow { get; set; }
    public static GameWindow GameWindow { get; set; }
    #endregion

    #region Commands
    public DelegateCommand GoNextCommand { get; } = new(ClickMethods.GoNext);
    public DelegateCommand GoBackCommand { get; } = new(ClickMethods.GoBack);
    public DelegateCommand QuitCommand { get; } = new(ClickMethods.Quit);
    public DelegateCommand StartGameCommand { get; } = new(StartGame);
    public DelegateCommand SelectSymbolCommand { get; } = new(SelectSymbol);
    public DelegateCommand DrawSymbolCommand { get; } = new(DrawSymbol);
    public DelegateCommand GoBackToMenuCommand { get; } = new(GoBackToMenu);

    #endregion

    #region Symbols
    public Symbol Cross { get => Symbol.Cross; private set => NotifyPropertyChanged(nameof(Cross)); }
    public Symbol Nought { get => Symbol.Nought; private set => NotifyPropertyChanged(nameof(Cross)); }
    #endregion

    #region Points
    public string Points
    {
        get => _gameResult.ToString();
        set => NotifyPropertyChanged(nameof(Points));
    }
    #endregion

    #region Difficulty properties

    public System.Windows.Media.Brush DifficultyColor
    {
        get => _difficultyColor;
        set => NotifyPropertyChanged(nameof(DifficultyColor));
    }

    public string DifficultyName
    {
        get => _difficultyName;
        set => NotifyPropertyChanged(nameof(DifficultyName));
    }

    #endregion

    #region Records
    public List<UserRecord> Records
    {
        get
        {
            using var records = new UserRecordsProxy();

            if (records.GetRecords().Count < 0)
            {
                throw new IndexOutOfRangeException();
            }

            return records.GetRecords();
        }
        private set => NotifyPropertyChanged(nameof(Records));
    }
    #endregion

    #region Players
    private static User? _user;
    private static Opponent? _opponent;
    #endregion

    #region Symbols strategies
    private static readonly Dictionary<Symbol, Func<ISymbolStrategy>> StrategyMap = new()
    {
        { Symbol.Cross, () => new CrossesStrategy() },
        { Symbol.Nought, () => new NoughtsStrategy() }
    };
    #endregion

    #region Game result

    private static int _gameResult;

    #endregion

    #region Difficulty fields

    private static System.Windows.Media.Brush _difficultyColor = System.Windows.Media.Brushes.GreenYellow;
    private static string _difficultyName = "Easy";

    #endregion

    #region User name

    private static string _userName = " ";

    #endregion

    private static readonly Dictionary<string, bool> DrawnCellsMap = new();
    private static Matrix _matrix;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public AppViewModel()
    {
        SoundsControl.StartSound.Play();
        _ = new UiRefresher(this);
    }

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static void StartGame(object? parameter)
    {
        if (StartWindow.LoginLabel.Content is not Grid loginGrid)
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

        StartWindow.Hide();
        ClickMethods.GoBack(StartWindow.LoginLabel);
        SoundsControl.StartSound.Stop();

        GameWindow.Height = StartWindow.ActualHeight;
        GameWindow.Width = StartWindow.ActualWidth;
        GameWindow.Left = StartWindow.Left;
        GameWindow.Top = StartWindow.Top;
        GameWindow.WindowState = StartWindow.WindowState;

        GameWindow.Show();

        SoundsControl.GameSound.Play();
        
        _matrix = new Matrix(3, GameWindow.Field);
    }

    private static void SelectSymbol(object? parameter)
    {
        if (parameter is not Symbol symbol)
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        var symbols = new[] { Symbol.Cross, Symbol.Nought };

        _user = new User(StrategyMap[symbol].Invoke(), _matrix);
        _opponent = new Opponent(StrategyMap[symbols.Single(x => x != symbol)].Invoke(), _matrix);

        ClickMethods.GoNext(GameWindow.GameUiContainer);

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

        Player.GameOver += winsCount =>
        {
            if (_gameResult != 0 && winsCount == _gameResult || _gameResult == 0 && winsCount == _gameResult)
            {
                var record = _gameResult;
                SetGameOver();

                using var records = new UserRecordsProxy();
                records.AddRecord(new UserRecord(_userName, records.GetRecords().Count + 1, record));
                UiRefresher.RefreshRecordsList();
            }

            _gameResult = winsCount;
            _difficultyColor = _opponent.CurrentDifficulty.Item1;
            _difficultyName = _opponent.CurrentDifficulty.Item2;

            DrawnCellsMap.Clear();
            
            UiRefresher.RefreshPoints(_gameResult);
            UiRefresher.RefreshDifficultyProperties(_difficultyColor, _difficultyName);
        };
    }

    private static void DrawSymbol(object? parameter)
    {
        if (parameter is not Button control)
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        if (DrawnCellsMap.TryGetValue(control.Name, out var isFilled))
        {
            if (isFilled)
            {
                return;
            }
        }
        
        var row = (int)control.GetValue(Grid.RowProperty);
        var column = (int)control.GetValue(Grid.ColumnProperty);

        _user?.Draw(row, column);
        
        DrawnCellsMap.Add(control.Name, true);
    }

    private static void SetGameOver()
    {
        SoundsControl.GameSound.Stop();
        SoundsControl.GameOverSound.Play();

        SoundsControl.GameOverSound.MediaEnded += (_, _) =>
        {
            ClickMethods.GoNext(GameWindow.GameOverLabel);
        };
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
