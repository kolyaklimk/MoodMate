using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using System.Collections.ObjectModel;
using MoodMate.Components;
using MoodMate.Pages.MoodNote;
using MoodMate.Components.Factory;

namespace MoodMate.ViewModels;

[QueryProperty(nameof(MoodNote), "Note")]
public partial class ChooseMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private readonly FileService Mood = new();
    public ObservableCollection<FileService> Moods { get; set; } = new();

    [ObservableProperty] FileService selectedMood;

    [RelayCommand] async void LoadMoodNote() => await Load();
    [RelayCommand] async void GoToCreateOrEditPage() => await GoToCreateOrEdit();

    private async Task GoToCreateOrEdit()
    {
        if (SelectedMood != null)
        {
            await Shell.Current.GoToAsync(nameof(CreateOrEditMoodPage),
                new Dictionary<string, object>() {
                    { "MoodNote", new MoodNote(){Mood = SelectedMood } },
                    { "Note", MoodNote}});
            SelectedMood = null;
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
