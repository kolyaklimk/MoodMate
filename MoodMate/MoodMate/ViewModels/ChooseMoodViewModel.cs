using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using System.Collections.ObjectModel;
using MoodMate.Components;

namespace MoodMate.ViewModels;

public partial class ChooseMoodViewModel: ObservableObject
{
    private readonly FileService Mood;
    public ObservableCollection<FileService> Moods { get; set; } = new();
    public ChooseMoodViewModel() 
    {
        Mood = new();
        LoadMoodNote();
    }

    [RelayCommand]
    async void LoadMoodNote() => await Load();
    private async Task Load()
    {
        await Mood.LoadService(Constants.PathMoods);
        var moods = Mood.GetServiceData();

        Moods.Clear();
        foreach(var mood in moods)
            Moods.Add(mood);
    }

}
