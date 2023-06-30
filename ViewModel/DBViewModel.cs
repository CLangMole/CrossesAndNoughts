using CrossesAndNoughts.Models;
using CrossesAndNoughts.Models.Strategies;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Media;

namespace CrossesAndNoughts.ViewModel
{
    public class DBViewModel : INotifyPropertyChanged
    {
        #region
        public DelegateCommand GoNextCommand { get => _goNextCommand; }
        public DelegateCommand GoBackCommand { get => _goBackCommand; }
        public DelegateCommand QuitCommand { get => _quitCommand; }
        public DelegateCommand StartGameCommand { get => _startGameCommand; }
        #endregion

        public static StartWindow? StartWindow { get; set; }
        public static GameWindow? GameWindow { get; set; }

        #region
        private readonly DelegateCommand _goNextCommand = new(ClickMethods.Instance.GoNext);
        private readonly DelegateCommand _goBackCommand = new(ClickMethods.Instance.GoBack);
        private readonly DelegateCommand _quitCommand = new(ClickMethods.Instance.Quit);
        private readonly DelegateCommand _startGameCommand = new(StartGame);

        #endregion

        private static readonly SoundPlayer _gameSound = new(Directory.GetCurrentDirectory() + @"\music-for-puzzle-game-146738.wav");
        private static readonly SoundPlayer _startSound = new(Directory.GetCurrentDirectory() + @"\Poofy Reel.wav");

        private static ISymbolStrategy? _symbolStrategy;

        public DBViewModel()
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
        }
    }
}
