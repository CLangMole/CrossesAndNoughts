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
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.ViewModel;

public class AppViewModel : INotifyPropertyChanged
{
    #region Windows
    public static StartWindow? StartWindow { get; set; }
    public static GameWindow? GameWindow { get; set; }
    #endregion

    #region Commands
    public DelegateCommand GoNextCommand { get => _goNextCommand; }
    public DelegateCommand GoBackCommand { get => _goBackCommand; }
    public DelegateCommand QuitCommand { get => _quitCommand; }
    public DelegateCommand StartGameCommand { get => _startGameCommand; }
    public DelegateCommand SelectSymbolCommand { get => _selectSymbolCommand; }
    public DelegateCommand DrawSymbolCommand { get => _drawSymbolCommand; }
    public DelegateCommand GoBackToMenuCommand { get => _goBackToMenuCommand; }
    #endregion

    #region Symbols
    public Symbol Cross { get => Symbol.Cross; private set => NotifyPropertyChanged(nameof(Cross)); }
    public Symbol Nought { get => Symbol.Nought; private set => NotifyPropertyChanged(nameof(Cross)); }
    #endregion

    #region Points
    public string Points
    {
        get
        {
            return _gameResult.ToString();
        }
        set
        {
            NotifyPropertyChanged(nameof(Points));
        }
    }
    #endregion

    #region Difficulty properties

    public System.Windows.Media.Brush DifficultyColor
    {
        get
        {
            return _difficultyColor;
        }
        set
        {
            NotifyPropertyChanged(nameof(DifficultyColor));
        }
    }

    public string DifficultyName
    {
        get
        {
            return _difficultyName;
        }
        set
        {
            NotifyPropertyChanged(nameof(DifficultyName));
        }
    }

    #endregion

    #region Records
    public List<UserRecord> Records
    {
        get
        {
            using IRecord? records = new UserRecordsProxy();

            if (records.GetRecords().Count < 0)
            {
                throw new IndexOutOfRangeException();
            }

            return records.GetRecords();
        }
        private set
        {
            NotifyPropertyChanged(nameof(Records));
        }
    }
    #endregion

    #region Private commands
    private readonly DelegateCommand _goNextCommand = new(ClickMethods.GoNext);
    private readonly DelegateCommand _goBackCommand = new(ClickMethods.GoBack);
    private readonly DelegateCommand _quitCommand = new(ClickMethods.Quit);
    private readonly DelegateCommand _startGameCommand = new(StartGame);
    private readonly DelegateCommand _selectSymbolCommand = new(SelectSymbol);
    private readonly DelegateCommand _drawSymbolCommand = new(DrawSymbol);
    private readonly DelegateCommand _goBackToMenuCommand = new(GoBackToMenu);
    #endregion

    #region Players
    private static User? _user;
    private static Opponent? _opponent;
    #endregion

    #region Symbols strategies
    private static readonly Dictionary<Symbol, Func<ISymbolStrategy>> _strategyMap = new()
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

    public event PropertyChangedEventHandler? PropertyChanged;

    public AppViewModel()
    {
        SoundsControl.StartSound.Play();
        UIRefresher viewModelHelper = new(this);
    }

    public void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static void StartGame(object? parameter)
    {
        if (StartWindow?.LoginLabel.Content is not Grid loginGrid)
        {
            throw new Exception($"Cannot find element {nameof(loginGrid)}");
        }

        if (loginGrid.Children[1] is not TextBox loginField)
        {
            throw new Exception($"Cannot find element {nameof(loginField)}");
        }

        if (string.IsNullOrEmpty(loginField.Text))
        {
            _userName = "Player" + new Random().Next(1, 1000000).ToString();
        }
        else
        {
            _userName = loginField.Text;
        }

        StartWindow?.Hide();
        ClickMethods.GoBack(StartWindow?.LoginLabel);
        SoundsControl.StartSound.Stop();

        GameWindow?.Show();

        SoundsControl.GameSound.Play();

        Player.Field = GameWindow?.Field;
        Matrix.Field = GameWindow?.Field;
    }

    private static void SelectSymbol(object? parameter)
    {
        if (parameter is not Symbol symbol)
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        Symbol[] symbols = new Symbol[2] { Symbol.Cross, Symbol.Nought };

        _user = new User(_strategyMap[symbol].Invoke());
        _opponent = new Opponent(_strategyMap[symbols.Single(x => x != symbol)].Invoke());

        ClickMethods.GoNext(GameWindow?.GameUIContainer);

        Matrix.Instance.CurrentUser = _user;
        Matrix.Instance.CurrentOpponent = _opponent;

        if (GameWindow is null)
        {
            throw new NullReferenceException(nameof(GameWindow));
        }

        if (_opponent.CurrentSymbol == Symbol.Cross)
        {
            Task draw = _opponent.Draw(-1, -1);
        }

        _user.UserDrawedSymbol += () =>
        {
            Task draw = _opponent.Draw(-1, -1);
            draw.Start();
        };

        Player.GameOver += (int winsCount) =>
        {
            if (_gameResult != 0 && winsCount == _gameResult || _gameResult == 0 && winsCount == _gameResult)
            {
                int record = _gameResult;

                SetGameOver();

                using IRecord? records = new UserRecordsProxy();
                records.AddRecord(new UserRecord(_userName, new Random().Next(0, 10), record));
                UIRefresher.RefreshRecordsList();
            }

            _gameResult = winsCount;
            _difficultyColor = _opponent.CurrentDifficulty.Item1;
            _difficultyName = _opponent.CurrentDifficulty.Item2;

            UIRefresher.RefreshPoints(_gameResult);
            UIRefresher.RefreshDifficultyProperties(_difficultyColor, _difficultyName);
        };
    }

    private static void DrawSymbol(object? parameter)
    {
        if (parameter is not Button control)
        {
            throw new ArgumentException(null, nameof(parameter));
        }

        int row = (int)control.GetValue(Grid.RowProperty);
        int column = (int)control.GetValue(Grid.ColumnProperty);

        _user?.Draw(row, column);
    }

    private static void SetGameOver()
    {
        SoundsControl.GameSound.Stop();
        SoundsControl.GameOverSound.Play();

        SoundsControl.GameOverSound.MediaEnded += (sender, e) =>
        {
            ClickMethods.GoNext(GameWindow?.GameOverLabel);
        };
    }

    private static void GoBackToMenu(object? parameter)
    {
        SoundsControl.GameSound.Stop();
        Matrix.Reset();
        _gameResult = 0;
        UIRefresher.RefreshPoints(_gameResult);
        GameWindow?.Hide();
        ClickMethods.GoBack(GameWindow?.GameGrid);
        StartWindow?.Show();

        SoundsControl.StartSound.Play();
    }

    private class UIRefresher
    {
        private static AppViewModel? _viewModel;

        internal UIRefresher(AppViewModel appViewModel)
        {
            _viewModel = appViewModel;
        }

        internal static void RefreshRecordsList()
        {
            if (_viewModel is null)
            {
                throw new NullReferenceException(nameof(_viewModel));
            }

            using IRecord? records = new UserRecordsProxy();

            _viewModel.Records = records.GetRecords();
        }

        internal static void RefreshPoints(int points)
        {
            if (_viewModel is null)
            {
                throw new NullReferenceException(nameof(_viewModel));
            }

            _viewModel.Points = points.ToString();
        }

        internal static void RefreshDifficultyProperties(System.Windows.Media.Brush brush, string name)
        {
            if (_viewModel is null)
            {
                throw new NullReferenceException(nameof(_viewModel));
            }

            _viewModel.DifficultyColor = brush;
            _viewModel.DifficultyName = name;
        }
    }
}
