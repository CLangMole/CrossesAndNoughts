using CrossesAndNoughts.Models;
using CrossesAndNoughts.Models.DataBase;
using CrossesAndNoughts.Models.Players;
using CrossesAndNoughts.Models.Strategies;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrossesAndNoughts.ViewModel
{
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
        #endregion

        #region Symbols
        public Symbol Cross { get => Symbol.Cross; private set => NotifyPropertyChanged(nameof(Cross)); }
        public Symbol Nought { get => Symbol.Nought; private set => NotifyPropertyChanged(nameof(Cross)); }
        #endregion

        #region Private commands
        private readonly DelegateCommand _goNextCommand = new(ClickMethods.Instance.GoNext);
        private readonly DelegateCommand _goBackCommand = new(ClickMethods.Instance.GoBack);
        private readonly DelegateCommand _quitCommand = new(ClickMethods.Instance.Quit);
        private readonly DelegateCommand _startGameCommand = new(StartGame);
        private readonly DelegateCommand _selectSymbolCommand = new(SelectSymbol);
        private readonly DelegateCommand _drawSymbolCommand = new(DrawSymbol);
        #endregion

        #region Sound players
        private static readonly SoundPlayer _gameSound = new(Directory.GetCurrentDirectory() + @"\music-for-puzzle-game-146738.wav");
        private static readonly SoundPlayer _startSound = new(Directory.GetCurrentDirectory() + @"\Poofy Reel.wav");
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

        private static readonly Matrix _fieldMatrix = new();

        public AppViewModel()
        {
            _startSound.PlayLooping();
            Player.FieldMatrix = _fieldMatrix;
        }

        public List<UserRecord> Records
        {
            get
            {
                using IRecord? records = new UserRecordsProxy();

                if (records.GetRecords().Count <= 0)
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void StartGame(object? parameter)
        {
            StartWindow?.Hide();
            _startSound.Stop();

            GameWindow?.Show();

            _gameSound.PlayLooping();

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

            ClickMethods.Instance.GoNext(GameWindow?.Field);

            if (_opponent.CurrentSymbol == Symbol.Cross)
            {
                Task draw = _opponent.Draw(-1, -1);
            }

            _user.UserDrawedSymbol += () =>
            {
                Task draw = _opponent.Draw(-1, -1);
                draw.Start();
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
    }
}
