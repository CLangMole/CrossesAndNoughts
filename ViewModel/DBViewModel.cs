using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CrossesAndNoughts;
using CrossesAndNoughts.Models;
using CrossesAndNoughts.View;
using CrossesAndNoughts.ViewModel.Commands;

namespace CrossesAndNoughts.ViewModel
{
    public class DBViewModel : INotifyPropertyChanged
    {
        public DelegateCommand GoNextCommand { get => _goNextCommand; }
        public DelegateCommand GoBackCommand { get => _goBackCommand; }
        public DelegateCommand QuitCommand { get => _quitCommand; }
        public DelegateCommand StartGameCommand { get => _startGameCommand; }

        #region
        private DelegateCommand _goNextCommand = new DelegateCommand(ClickMethods.GoNext);
        private DelegateCommand _goBackCommand = new DelegateCommand(ClickMethods.GoBack);
        private DelegateCommand _quitCommand = new DelegateCommand(ClickMethods.Quit);
        private DelegateCommand _startGameCommand = new DelegateCommand(StartGame);

        #endregion

        private static SoundPlayer _gameSound = new SoundPlayer(@"C:\Users\probn\Fiverr\FiverrAssets\music-for-puzzle-game-146738.wav");
        private static SoundPlayer _startSound = new SoundPlayer(@"C:\Users\probn\Fiverr\FiverrAssets\Poofy Reel.wav");

        private static StartWindow? _startWindow;
        private static GameWindow? _gameWindow;

        public DBViewModel(StartWindow startWindow, GameWindow gameWindow)
        {
            _startWindow = startWindow;
            _gameWindow = gameWindow;
            _startSound.PlayLooping();
        }

        private List<UserRecord> _records()
        {
            using (IRecord records = new UserRecordsProxy())
            {
                if (records.GetRecords().Count <= 0)
                    throw new Exception();
                return records.GetRecords();
            }
        }

        public List<UserRecord> Records
        {
            get { return _records(); }
            private set
            {
                Records = value;
                NotifyPropertyChanged(nameof(Records));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void StartGame(object? parameter)
        {
            _startWindow?.Hide();
            _startSound.Stop();

            _gameWindow?.Show();

            _gameSound.PlayLooping();
        }
    }
}
