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
        private readonly DelegateCommand _goNextCommand = new(ClickMethods.GoNext);
        private readonly DelegateCommand _goBackCommand = new(ClickMethods.GoBack);
        private readonly DelegateCommand _quitCommand = new(ClickMethods.Quit);
        private readonly DelegateCommand _startGameCommand = new(StartGame);
        private readonly DelegateCommand _selectSymbolCommand = new(SelectSymbol);
        private readonly DelegateCommand _drawSymbolCommand = new(DrawSymbol);
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

        #region User name

        private static string _userName = " ";

        #endregion

        public AppViewModel()
        {
            SoundsControl.StartSound.Play();
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

            ClickMethods.GoNext(GameWindow?.GameCanvas); // GameWindow?.Field

            Matrix.Instance.CurrentUser = _user;
            Matrix.Instance.CurrentOpponent = _opponent;

            if (_opponent.CurrentSymbol == Symbol.Cross)
            {
                Task draw = _opponent.Draw(-1, -1);
            }

            _user.UserDrawedSymbol += () =>
            {
                Task draw = _opponent.Draw(-1, -1);
                draw.Start();
            };

            Player.Won += (int winsCount) =>
            {
                if (_gameResult != 0 && winsCount == _gameResult || _gameResult == 0 && winsCount == _gameResult)
                {
                    int record = _gameResult;

                    SetGameOver();

                    using IRecord? records = new UserRecordsProxy();
                    records.AddRecord(new UserRecord(_userName, new Random().Next(0, 10), record));
                    
                }

                _gameResult = winsCount;

                var textBlock = (TextBlock)GameWindow.GameCanvas.Children[1];
                textBlock.Text = _gameResult.ToString();
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
                Matrix.Reset();
                _gameResult = 0;
                GameWindow?.Hide();
                StartWindow?.Show();

                SoundsControl.StartSound.Play();
            };
        }
    }
}
