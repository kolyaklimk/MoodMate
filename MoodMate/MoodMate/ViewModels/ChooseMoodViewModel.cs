using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using System.Collections.ObjectModel;
using MoodMate.Components;
using MoodMate.Pages.MoodNote;

namespace MoodMate.ViewModels;

public partial class ChooseMoodViewModel : ObservableObject
{
    private readonly FileService Mood;
    public ObservableCollection<FileService> Moods { get; set; } = new();
    public ChooseMoodViewModel()
    {
        Mood = new();
        LoadMoodNote();
    }

    [ObservableProperty] FileService selectedMood;

    [RelayCommand] async void LoadMoodNote() => await Load();

    [RelayCommand] async void GoToCreateOrEditPage() => await GoToCreateOrEdit();

    private async Task GoToCreateOrEdit()
    {
        if (SelectedMood != null)
        {
            SelectedMood = null;
            await Shell.Current.GoToAsync(nameof(CreateOrEditMoodPage),
                new Dictionary<string, object>() { { "Mood", SelectedMood } });
        }
    }
    private async Task Load()
    {
        await Mood.LoadService(Constants.PathMoods);
        var moods = Mood.GetServiceData();

        Moods.Clear();
        foreach (var mood in moods)
            Moods.Add(mood);
    }

}
