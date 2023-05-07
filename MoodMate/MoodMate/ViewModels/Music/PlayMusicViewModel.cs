using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MoodMate.Components.Entities;
using MoodMate.Pages.Music;
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
    [ObservableProperty] string buttonStr;
    [ObservableProperty] TimeSpan time;
    [ObservableProperty] double volumeMusic;
    [ObservableProperty] double volumeSound;

    public PlayMusicViewModel(IAudioManager manager)
    {
        AudioManager = manager;

        var second = TimeSpan.FromSeconds(1);
        Timer.Interval = TimeSpan.FromSeconds(1);
        Timer.Tick += (s, e) =>
        {
            if (Time == zero)
            {
                MessagingCenter.Send(this, "Stop");
                MusicPlayer?.Pause();
                SoundPlayer?.Pause();
                Timer.Stop();
            }
            else
            {
                MessagingCenter.Send(this, "Start");
                Time = Time.Subtract(second);
            }
        };
    }

    [RelayCommand]
    async void LoadPage()
    {
        if (SelectedMusic != null && SelectedMusic.Name != "Silence")
        {
            IsMusic = true;
            MusicPlayer = AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(SelectedMusic.Source));
            MusicPlayer.Loop = true;
        }

        if (SelectedSound != null && SelectedSound?.Name != "Silence")
        {
            IsSound = true;
            SoundPlayer = AudioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(SelectedSound.Source));
            SoundPlayer.Loop = true;
        }

        ButtonStr = "Stop";
        VolumeMusic = 1;
        VolumeSound = 1;

        MessagingCenter.Send(this, "Start");
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
                MessagingCenter.Send(this, "Stop");
                ButtonStr = "Continue";
                MusicPlayer?.Pause();
                SoundPlayer?.Pause();
                Timer.Stop();
                IsRunning = false;
            }
            else
            {
                MessagingCenter.Send(this, "Start");
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
