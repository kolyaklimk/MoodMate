using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class MoodListViewModel: ObservableObject
{
    private readonly Note MoodNote;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();
    public MoodListViewModel()
    {
        MoodNote = new("Mood");
        MoodNotes = MoodNote.note.GetData();
        LoadMoodNote();
    }

    [RelayCommand]
    async void LoadMoodNote() => await Load();
    private async Task Load()
    {
        await MoodNote.note.LoadNote();
    }
}
