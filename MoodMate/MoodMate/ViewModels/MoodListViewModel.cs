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

        MessagingCenter.Subscribe<CreateOrEditMoodViewModel>(this,
            "UpdateMoodNote", (sender) => UpdateMoodNote());
    }

    [RelayCommand]
    async void LoadMoodNote()
    {
        await MoodNote.note.LoadNote();
        UpdateMoodNote();
    }

    [RelayCommand]
    async void GoToChooseMoodPage()
    {
        await Shell.Current.GoToAsync(nameof(ChooseMoodPage));
    }

    [RelayCommand]
    void UpdateMoodNote()
    {
        var moods = MoodNote.note.GetData();

        MoodNotes.Clear();
        foreach (var mood in moods)
            MoodNotes.Add(mood);
    }

    [RelayCommand]
    void Popup(MoodNote note)
    {
    }
}
