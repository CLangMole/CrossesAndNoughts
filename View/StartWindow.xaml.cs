using System.Media;
using System.Windows;
using System.Windows.Input;
using CrossesAndNoughts;
using CrossesAndNoughts.ViewModel;

namespace CrossesAndNoughts.View;

public partial class StartWindow : Window
{
    private GameWindow _gameWindow;
    //private string _userName = string.Empty;
    public StartWindow(GameWindow gameWindow, DBViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _gameWindow = gameWindow;

        //SoundPlayer startSoundPlayer = new SoundPlayer(@"C:\Users\probn\Fiverr\FiverrAssets\Poofy Reel.wav");
        //startSoundPlayer.PlayLooping();

        //Closed += (sender, e) => Application.Current.Shutdown();
        //RecordsBackButton.Click += (sender, e) => GoBack(RecordsLabel);
        //LoginBackButton.Click += (sender, e) => GoBack(LoginLabel);
        //StartGameButton.Click += (sender, e) => StartGame(ref startSoundPlayer);
    }

    //private void StartButton_Click(object sender, RoutedEventArgs e)
    //{
    //    GoNext(LoginLabel);
    //}

    //private void RecordsButton_Click(object sender, RoutedEventArgs e)
    //{
    //    GoNext(RecordsLabel);
    //}

    //private void QuitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

    //private void GoNext(params UIElement[] nextControls)
    //{
    //    foreach (UIElement nextControl in nextControls)
    //    {
    //        if (nextControl == null) return;
    //        nextControl.Visibility = Visibility.Visible;

    //        if (LayoutGrid.Children.Count < 1) return;
    //        foreach (UIElement childrenControl in LayoutGrid.Children)
    //        {
    //            if (childrenControl == nextControl || childrenControl.Uid == "CollapsedAtStart") continue;
    //            childrenControl.Visibility = Visibility.Collapsed;
    //        }
    //    }
    //}

    //private void GoBack(params UIElement[] currentControls)
    //{
    //    foreach (UIElement currentControl in currentControls)
    //    {

    //        if (currentControl == null) return;

    //        if (LayoutGrid.Children.Count == 0) return;

    //        foreach (UIElement childrenControl in LayoutGrid.Children)
    //        {
    //            if (childrenControl == currentControl || childrenControl.Uid == "CollapsedAtStart") continue;
    //            childrenControl.Visibility = Visibility.Visible;
    //        }

    //        currentControl.Visibility = Visibility.Collapsed;
    //    }
    //}

    //private void StartGame(ref SoundPlayer soundPlayer)
    //{
    //    Hide();
    //    soundPlayer.Stop();

    //    _gameWindow.Show();

    //    SoundPlayer mainSoundPlayer = new SoundPlayer(@"C:\Users\probn\Fiverr\FiverrAssets\music-for-puzzle-game-146738.wav");
    //    mainSoundPlayer.PlayLooping();
    //}
}
