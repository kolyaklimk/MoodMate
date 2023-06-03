using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels.Music;

public partial class MusicListViewModel : ObservableObject
{
    private readonly FileService Music;
    private readonly FileService Sound;
    private readonly List<int>[] Sbytes;
    private readonly IToast Alert;
    public ObservableCollection<FileService> MusicList { get; set; } = new();
    public ObservableCollection<FileService> SoundList { get; set; } = new();

    [ObservableProperty] FileService selectedMusic;
    [ObservableProperty] FileService selectedSound;
    [ObservableProperty] TimeSpan selectedTime;

    public MusicListViewModel(FileService[] fileService, List<int>[] sbytes, IToast[] toasts)
    {
        Music = fileService[1];
        Sound = fileService[2];
        Sbytes = sbytes;
        Alert = toasts[3];
    }

    [RelayCommand]
    void SelectedItemMusic(FileService music)
    {
        if (SelectedMusic == music)
        {
            SelectedMusic = null;
        }
        else
        {
            SelectedMusic = music;
        }
    }

    [RelayCommand]
    void SelectedItemSound(FileService sound)
    {
        if (SelectedSound == sound)
        {
            SelectedSound = null;
        }
        else
        {
            SelectedSound = sound;
        }
    }

    [RelayCommand]
    async Task ChooseTime()
    {
        var result = await Shell.Current.ShowPopupAsync(new TimePickerPage(Sbytes));
        if (result != null)
        {
            SelectedTime = TimeSpan.Parse((string)result);
        }
    }

    [RelayCommand]
    public async Task GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
        SelectedMusic = null;
        SelectedSound = null;
    }

    [RelayCommand]
    async Task GoToNotePage()
    {
        await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
        SelectedMusic = null;
        SelectedSound = null;
    }

    [RelayCommand]
    async Task LoadPage()
    {
        await Task.Run(() =>
        {
            SelectedTime = new(0, 1, 0);

            foreach (var m in Music.GetServiceData())
                MusicList.Add(m);

            foreach (var s in Sound.GetServiceData())
                SoundList.Add(s);
        });
    }

    [RelayCommand]
    async Task GoToPlay()
    {
        if (SelectedMusic != null || SelectedSound != null)
        {
            await Shell.Current.GoToAsync($"//{nameof(PlayMusicPage)}",
                new Dictionary<string, object>() {
                    { "SelectedMusic", SelectedMusic},
                    { "SelectedSound", SelectedSound},
                    { "Time", SelectedTime}});
            SelectedMusic = null;
            SelectedSound = null;
        }
        else
        {
            await Alert.Show();
        }
    }
}