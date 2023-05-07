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
    public MusicListViewModel(FileService[] fileService)
    {
        Music = fileService[1];
        Sound = fileService[2];
    }
    public ObservableCollection<FileService> MusicList { get; set; } = new();
    public ObservableCollection<FileService> SoundList { get; set; } = new();

    [ObservableProperty] FileService selectedMusic;
    [ObservableProperty] FileService selectedSound;
    [ObservableProperty] TimeSpan selectedTime = new(0, 1, 0);

    [RelayCommand]
    async void ChooseTime()
    {
        var result = await Shell.Current.ShowPopupAsync(new TimePickerPage());
        if (result != null)
        {
            SelectedTime = TimeSpan.Parse((string)result);
        }
    }

    [RelayCommand]
    async void GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
    }

    [RelayCommand]
    async void GoToNotePage()
    {
        await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
    }
    [RelayCommand]
    void LoadPage()
    {
        var M = Music.GetServiceData();
        var S = Sound.GetServiceData();

        foreach (var m in M)
        {
            MusicList.Add(m);
        }
        foreach (var s in S)
        {
            SoundList.Add(s);
        }
    }

    [RelayCommand]
    async void GoToPlay()
    {
        if (SelectedMusic != null || SelectedSound != null)
        {
            await Shell.Current.GoToAsync(nameof(PlayMusicPage),
                new Dictionary<string, object>() {
                    { "SelectedMusic", SelectedMusic},
                    { "SelectedSound", SelectedSound},
                    { "Time", SelectedTime}});
            SelectedMusic = null;
            SelectedSound = null;
        }
    }
}