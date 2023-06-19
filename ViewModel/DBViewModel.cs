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

        public static StartWindow? StartWindow { get; set; }
        public static GameWindow? GameWindow { get; set; }

        #region
        private DelegateCommand _goNextCommand = new(ClickMethods.GoNext);
        private DelegateCommand _goBackCommand = new(ClickMethods.GoBack);
        private DelegateCommand _quitCommand = new(ClickMethods.Quit);
        private DelegateCommand _startGameCommand = new(StartGame);

        #endregion

        private static SoundPlayer _gameSound = new(@"C:\Users\probn\Fiverr\FiverrAssets\music-for-puzzle-game-146738.wav");
        private static SoundPlayer _startSound = new(@"C:\Users\probn\Fiverr\FiverrAssets\Poofy Reel.wav");


        public DBViewModel()
        {
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
            StartWindow?.Hide();
            _startSound.Stop();

            GameWindow?.Show();

            _gameSound.PlayLooping();
        }
    }
}
