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
<<<<<<< HEAD
    #region Commands

    public DelegateCommand GoNextCommand { get; } = new(ClickMethods.GoNext);
    public DelegateCommand GoBackCommand { get; } = new(ClickMethods.GoBack);
    public DelegateCommand QuitCommand { get; } = new(ClickMethods.Quit);
    public DelegateCommand GoBackToMenuCommand { get; } = new(GoBackToMenu);

    public DelegateCommand StartGameCommand { get; }
    public DelegateCommand SelectSymbolCommand { get; }
    public DelegateCommand DrawSymbolCommand { get; }

=======
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

>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
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
<<<<<<< HEAD
        set
        {
            _gameResult = int.Parse(value);
            NotifyPropertyChanged(nameof(Points));
        }
=======
        set => NotifyPropertyChanged(nameof(Points));
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    }

    public System.Windows.Media.Brush DifficultyColor
    {
        get => _difficultyColor;
<<<<<<< HEAD
        set
        {
            _difficultyColor = value;
            NotifyPropertyChanged(nameof(DifficultyColor));
        }
=======
        set => NotifyPropertyChanged(nameof(DifficultyColor));
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    }

    public string DifficultyName
    {
        get => _difficultyName;
<<<<<<< HEAD
        set
        {
            _difficultyName = value;
            NotifyPropertyChanged(nameof(DifficultyName));
        }
=======
        set => NotifyPropertyChanged(nameof(DifficultyName));
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    }

    public List<UserRecord> Records
    {
<<<<<<< HEAD
        get => _records;
        private set
        {
            _records = value;
            NotifyPropertyChanged(nameof(Records));
        }
=======
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
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    }

<<<<<<< HEAD
    #endregion
    
    #region Fields

    public event PropertyChangedEventHandler? PropertyChanged;
    
    private List<UserRecord> _records = [];
    
    private readonly StartWindow _startWindow;
    private readonly GameWindow _gameWindow;
    
    private User? _user;
    private Opponent? _opponent;

    private readonly Dictionary<Symbol, Func<ISymbolStrategy>> _strategyMap = new()
=======
    #region Players
    private static User? _user;
    private static Opponent? _opponent;
    #endregion

    #region Symbols strategies
    private static readonly Dictionary<Symbol, Func<ISymbolStrategy>> StrategyMap = new()
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    {
        { Symbol.Cross, () => new CrossesStrategy() },
        { Symbol.Nought, () => new NoughtsStrategy() }
    };

    private int _gameResult;

    private System.Windows.Media.Brush _difficultyColor = System.Windows.Media.Brushes.GreenYellow;
    private string _difficultyName = "Easy";

    private string _userName = " ";

    private readonly Matrix _matrix;

    #endregion

<<<<<<< HEAD
    public AppViewModel(StartWindow startWindow, GameWindow gameWindow)
    {
        _startWindow = startWindow;
        _gameWindow = gameWindow;

        _startWindow.DataContext = this;
        _gameWindow.DataContext = this;
        
        _matrix = new Matrix(3, _gameWindow.Field);
        
        SoundsControl.StartSound.Play();
        _ = new UiRefresher(this);

        StartGameCommand = new DelegateCommand(StartGame);
        SelectSymbolCommand = new DelegateCommand(SelectSymbol);
        DrawSymbolCommand = new DelegateCommand(DrawSymbol);
    }
    
=======
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

>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void StartGame(object? parameter)
    {
<<<<<<< HEAD
        if (_startWindow.LoginLabel.Content is not Grid loginGrid)
=======
        if (StartWindow.LoginLabel.Content is not Grid loginGrid)
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
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
<<<<<<< HEAD
=======
        
        _matrix = new Matrix(3, GameWindow.Field);
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
    }

    private void SelectSymbol(object? parameter)
    {
        if (parameter is not Symbol symbol)
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        var symbols = new[] { Symbol.Cross, Symbol.Nought };

<<<<<<< HEAD
        _user = new User(_strategyMap[symbol].Invoke(), _matrix);
        _opponent = new Opponent(_strategyMap[symbols.Single(x => x != symbol)].Invoke(), _matrix);

        ClickMethods.GoNext(_gameWindow.GameUiContainer);
=======
        _user = new User(StrategyMap[symbol].Invoke(), _matrix);
        _opponent = new Opponent(StrategyMap[symbols.Single(x => x != symbol)].Invoke(), _matrix);

        ClickMethods.GoNext(GameWindow.GameUiContainer);
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef

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
<<<<<<< HEAD
                Records = records.GetRecords();
            }

            Points = winsCount.ToString();
            DifficultyColor = _opponent.CurrentDifficulty.Item1;
            DifficultyName = _opponent.CurrentDifficulty.Item2;
=======
                UiRefresher.RefreshRecordsList();
            }

            _gameResult = winsCount;
            _difficultyColor = _opponent.CurrentDifficulty.Item1;
            _difficultyName = _opponent.CurrentDifficulty.Item2;

            DrawnCellsMap.Clear();
            
            UiRefresher.RefreshPoints(_gameResult);
            UiRefresher.RefreshDifficultyProperties(_difficultyColor, _difficultyName);
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
        };
    }

    private void DrawSymbol(object? parameter)
    {
        if (parameter is not Button control)
        {
            throw new ArgumentException(null, nameof(parameter));
        }
<<<<<<< HEAD
=======

        if (DrawnCellsMap.TryGetValue(control.Name, out var isFilled))
        {
            if (isFilled)
            {
                return;
            }
        }
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
        
        var row = (int)control.GetValue(Grid.RowProperty);
        var column = (int)control.GetValue(Grid.ColumnProperty);

        _user?.Draw(row, column);
        
        DrawnCellsMap.Add(control.Name, true);
    }

    private void SetGameOver()
    {
        SoundsControl.GameSound.Stop();
        SoundsControl.GameOverSound.Play();

        SoundsControl.GameOverSound.MediaEnded += (_, _) =>
        {
<<<<<<< HEAD
            ClickMethods.GoNext(_gameWindow.GameOverLabel);
=======
            ClickMethods.GoNext(GameWindow.GameOverLabel);
>>>>>>> f99d01646a33213fca7063b7683d7185685e53ef
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
