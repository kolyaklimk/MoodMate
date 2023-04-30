using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class MoodListViewModel : ObservableObject
{
    private readonly Note MoodNote;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();

    public MoodListViewModel(Note[] note)
    {
        MoodNote = note[0];
    }

    [RelayCommand]
    async void LoadMoodNote()
    {
        await MoodNote.note.LoadNote();
        var moods = MoodNote.note.GetData();

        MoodNotes.Clear();
        foreach (var mood in moods)
            MoodNotes.Add(mood);
    }

    [RelayCommand]
    async void GoToChooseMoodPage()
    {
        await Shell.Current.GoToAsync(nameof(ChooseMoodPage));
    }
}
