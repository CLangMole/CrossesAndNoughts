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
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace CrossesAndNoughts.ViewModel
{
    public class AppViewModel : INotifyPropertyChanged
    {
        #region
        public DelegateCommand GoNextCommand { get => _goNextCommand; }
        public DelegateCommand GoBackCommand { get => _goBackCommand; }
        public DelegateCommand QuitCommand { get => _quitCommand; }
        public DelegateCommand StartGameCommand { get => _startGameCommand; }
        public DelegateCommand SelectSymbolCommand { get => _selectSymbolCommand; }
        public DelegateCommand DrawSymbolCommand { get => _drawSymbolCommand; }
        #endregion

        public static StartWindow? StartWindow { get; set; }
        public static GameWindow? GameWindow { get; set; }

        #region
        private readonly DelegateCommand _goNextCommand = new(ClickMethods.Instance.GoNext);
        private readonly DelegateCommand _goBackCommand = new(ClickMethods.Instance.GoBack);
        private readonly DelegateCommand _quitCommand = new(ClickMethods.Instance.Quit);
        private readonly DelegateCommand _startGameCommand = new(StartGame);
        private readonly DelegateCommand _selectSymbolCommand = new(SelectSymbol);
        private readonly DelegateCommand _drawSymbolCommand = new(DrawSymbol);

        #endregion

        private static readonly SoundPlayer _gameSound = new(Directory.GetCurrentDirectory() + @"\music-for-puzzle-game-146738.wav");
        private static readonly SoundPlayer _startSound = new(Directory.GetCurrentDirectory() + @"\Poofy Reel.wav");

        private static ISymbolStrategy? _symbolStrategy;

        private static User? _user;

        public AppViewModel()
        {
            _startSound.PlayLooping();
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

            _symbolStrategy = new CrossesStrategy();
            _symbolStrategy.DrawSymbol(GameWindow?.Field, 1, 1);
            _symbolStrategy = new NoughtsStrategy();
            _symbolStrategy.DrawSymbol(GameWindow?.Field, 0, 1);

            Player.Field = GameWindow?.Field;
            _user ??= new User(new CrossesStrategy());
        }

        private static void SelectSymbol(object? parameter)
        {
            if (parameter is not Symbol symbol)
            {
                throw new ArgumentException(nameof(parameter));
            }

            Dictionary<Symbol, Func<User>> user = new()
            {
                { Symbol.Cross, () => new User(new CrossesStrategy()) },
                { Symbol.Nought, () => new User(new NoughtsStrategy()) }
            };

            _user = user[symbol].Invoke();
        }

        private static void DrawSymbol(object? parameter)
        {
            if (parameter is not UIElement control)
            {
                throw new ArgumentException(null, nameof(parameter));
            }

            int row = (int)control.GetValue(Grid.RowProperty);
            int column = (int)control.GetValue(Grid.ColumnProperty);

            _user?.Draw(row, column);
        }
    }
}
