using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MoodMate.Components.Entities;
using Plugin.Maui.Audio;

namespace MoodMate.ViewModels.Music;

[QueryProperty(nameof(SelectedMusic), "SelectedMusic")]
[QueryProperty(nameof(SelectedSound), "SelectedSound")]
[QueryProperty(nameof(Time), "Time")]
public partial class PlayMusicViewModel : ObservableObject
{
    private IDispatcherTimer Timer = Shell.Current.Dispatcher.CreateTimer();
    private TimeSpan zero = TimeSpan.FromSeconds(0);
    private IAudioManager AudioManager;
    private IAudioPlayer MusicPlayer;
    private IAudioPlayer SoundPlayer;
    private bool IsRunning;

    [ObservableProperty] FileService selectedMusic;
    [ObservableProperty] FileService selectedSound;
    [ObservableProperty] int rotate;
    [ObservableProperty] bool isMusic;
    [ObservableProperty] bool isSound;
    [ObservableProperty] string buttonStr = "Stop";
    [ObservableProperty] TimeSpan time;
    [ObservableProperty] double volumeMusic = 1;
    [ObservableProperty] double volumeSound = 1;

    public PlayMusicViewModel(IAudioManager manager)
    {
        AudioManager = manager;

        var second = TimeSpan.FromSeconds(1);
        Timer.Interval = TimeSpan.FromSeconds(1);
        Timer.Tick += (s, e) =>
        {
            if (Time == zero)
            {
                MusicPlayer?.Pause();
                SoundPlayer?.Pause();
                Timer.Stop();
            }
            else
            {
                Time = Time.Subtract(second);
            }
        };
    }

    [RelayCommand]
    async void LoadPage()
    {
        if (SelectedMusic != null)
        {
            IsMusic = true;
            MusicPlayer = AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(SelectedMusic.Source));
            MusicPlayer.Loop = true;

        }
        if (SelectedSound != null)
        {
            IsSound = true;
            SoundPlayer = AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(SelectedSound.Source));
            SoundPlayer.Loop = true;
        }

        MusicPlayer?.Play();
        SoundPlayer?.Play();
        Timer.Start();
        IsRunning = true;
    }

    [RelayCommand]
    void ButtonClick()
    {
        if (Time != zero)
        {
            if (IsRunning)
            {
                ButtonStr = "Continue";
                MusicPlayer?.Pause();
                SoundPlayer?.Pause();
                Timer.Stop();
                IsRunning = false;
            }
            else
            {
                ButtonStr = "Stop";
                MusicPlayer?.Play();
                SoundPlayer?.Play();
                Timer.Start();
                IsRunning = true;
            }
        }
    }

    [RelayCommand]
    async void BackClick()
    {
        await Shell.Current.Navigation.PopAsync();
        MusicPlayer?.Dispose();
        SoundPlayer?.Dispose();
        Timer.Stop();
        SelectedMusic = null;
        SelectedSound = null; 
        VolumeMusic = 1;
        VolumeSound = 1;
    }

    [RelayCommand]
    void ChangeMusicVolume()
    {
        if (MusicPlayer != null)
            MusicPlayer.Volume = VolumeMusic;
    }

    [RelayCommand]
    void ChangeSoundVolume()
    {
        if (SoundPlayer != null)
            SoundPlayer.Volume = VolumeSound;
    }
}
