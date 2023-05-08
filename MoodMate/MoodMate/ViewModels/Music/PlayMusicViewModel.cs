using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Messages;
using Plugin.Maui.Audio;
using System.Timers;

namespace MoodMate.ViewModels.Music;

[QueryProperty(nameof(SelectedMusic), "SelectedMusic")]
[QueryProperty(nameof(SelectedSound), "SelectedSound")]
[QueryProperty(nameof(Time), "Time")]
public partial class PlayMusicViewModel : ObservableObject
{
    private System.Timers.Timer Timer;
    private TimeSpan Zero;
    private TimeSpan Second;
    private IAudioManager AudioManager;
    private IAudioPlayer MusicPlayer;
    private IAudioPlayer SoundPlayer;
    private bool IsRunning;
    private readonly StartRotateMessage StartRotateMessage;
    private readonly StopRotateMessage StopRotateMessage;

    [ObservableProperty] FileService selectedMusic;
    [ObservableProperty] FileService selectedSound;
    [ObservableProperty] bool isMusic;
    [ObservableProperty] bool isSound;
    [ObservableProperty] string buttonStr;
    [ObservableProperty] TimeSpan time;
    [ObservableProperty] double volumeMusic;
    [ObservableProperty] double volumeSound;
    [ObservableProperty] bool isLoad;

    public PlayMusicViewModel(IAudioManager manager, StopRotateMessage stop, StartRotateMessage start)
    {
        AudioManager = manager;
        isLoad = true;
        StopRotateMessage = stop;
        StartRotateMessage = start;
    }
    private void SetTimer()
    {
        Timer = new(Second);
        Timer.Elapsed += (sender, e) =>
        {
            if (Time == Zero)
            {
                MusicPlayer?.Pause();
                SoundPlayer?.Pause();
                Timer.Stop();
                WeakReferenceMessenger.Default.Send(StopRotateMessage);
            }
            else
            {
                Time = Time.Subtract(Second);
                WeakReferenceMessenger.Default.Send(StartRotateMessage);
            }
        };
    }

    [RelayCommand]
    async Task LoadPage()
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
        Zero = TimeSpan.FromSeconds(0);
        Second = TimeSpan.FromSeconds(1);

        SetTimer();

        WeakReferenceMessenger.Default.Send(StartRotateMessage);
        MusicPlayer?.Play();
        SoundPlayer?.Play();
        Timer.Start();
        IsRunning = true;
        IsLoad = false;
    }

    [RelayCommand]
    void ButtonClick()
    {
        if (Time != Zero)
        {
            if (IsRunning)
            {
                WeakReferenceMessenger.Default.Send(StopRotateMessage);
                ButtonStr = "Continue";
                MusicPlayer?.Pause();
                SoundPlayer?.Pause();
                Timer.Stop();
                IsRunning = false;
            }
            else
            {
                WeakReferenceMessenger.Default.Send(StartRotateMessage);
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
