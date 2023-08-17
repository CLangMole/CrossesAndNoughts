using System;
using System.IO;
using System.Windows.Media;

namespace CrossesAndNoughts.Models;

public static class SoundsControl
{
    public static readonly MediaPlayer GameSound = new();
    public static readonly MediaPlayer StartSound = new();
    public static readonly MediaPlayer ClickSound = new();
    public static readonly MediaPlayer GameOverSound = new();
    public static readonly MediaPlayer WinSound = new();

    static SoundsControl()
    {
        GameSound.Open(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "music-for-puzzle-game-146738.wav")));
        StartSound.Open(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Poofy Reel.wav")));
        ClickSound.Open(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Click.wav")));
        GameOverSound.Open(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "game-over.wav")));
        WinSound.Open(new Uri(Path.Combine(Directory.GetCurrentDirectory(), "Win.mp3")));

        GameSound.MediaEnded += (sender, e) => OnGameSoundEnded(sender, e);
        StartSound.MediaEnded += (sender, e) => OnStartSoundEnded(sender, e);

        ClickSound.MediaEnded += (sender, e) =>
        {
            ClickSound.Stop();
        };

        WinSound.MediaEnded += (sender, e) =>
        {
            WinSound.Stop();
        };
    }

    private static void OnGameSoundEnded(object? sender, EventArgs e)
    {
        GameSound.Position = TimeSpan.Zero;
        GameSound.Play();
    }

    private static void OnStartSoundEnded(object? sender, EventArgs e)
    {
        StartSound.Position = TimeSpan.Zero;
        StartSound.Play();
    }
}
