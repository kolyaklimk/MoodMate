using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using System.Collections.ObjectModel;
using MoodMate.Components;
using MoodMate.Pages.MoodNote;
using MoodMate.Components.Factory;

namespace MoodMate.ViewModels;

public partial class ChooseMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private readonly FileService Mood = new();
    public ChooseMoodViewModel(Note[] note)
    {
        MoodNote = note[0];
    }
    public ObservableCollection<FileService> Moods { get; set; } = new();

    [ObservableProperty] FileService selectedMood;
    [ObservableProperty] DateTime dateTime = DateTime.Now;

    [RelayCommand]
    async void LoadMoodNote()
    {
        await Mood.LoadService(Constants.PathMoods);
        var moods = Mood.GetServiceData();

        Moods.Clear();
        foreach (var mood in moods)
            Moods.Add(mood);
    }

    [RelayCommand]
    async void GoToCreateOrEditPage()
    {
        OnPropertyChanged(nameof(MoodNote));
        if (SelectedMood != null)
        {
            await Shell.Current.GoToAsync(nameof(CreateOrEditMoodPage),
                new Dictionary<string, object>() {
                    { "MoodNote", new MoodNote() { Mood = SelectedMood, Date = DateTime }},
                    { "Create", true}});
            SelectedMood = null;
        }
    }
}
