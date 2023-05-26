using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.SimpleNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class NoteListViewModel : ObservableObject, IRecipient<UpdateSimpleNoteMessage>
{

    private readonly Note SimpleNote;
    public ObservableCollection<SimpleNote> SimpleNotes { get; set; } = new();

    public NoteListViewModel(Note[] note)
    {
        SimpleNote = note[1];
        IsRefreshing = false;

        WeakReferenceMessenger.Default.Register(this);
    }

    [ObservableProperty] bool isRefreshing;

    [RelayCommand]
    async Task GoToMusicPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MusicListPage));
    }

    [RelayCommand]
    public async Task GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
    }

    [RelayCommand]
    async Task GoToEditPage(SimpleNote note)
    {
        await Shell.Current.GoToAsync("//" + nameof(CreateOrEditNotePage),
            new Dictionary<string, object>() {
                {"SimpleNote", note},
                {"Save", new SimpleNote(note.Date, note.Text)}});
    }

    [RelayCommand]
    async Task GoToCreateOrEditPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(CreateOrEditNotePage),
                new Dictionary<string, object>() {
                    { "SimpleNote", new SimpleNote() { Date = DateTime.Now }},
                    { "Create", true}});
    }

    [RelayCommand]
    async Task UpdateSimpleNote()
    {
        await Task.Run(() =>
        {
            var moods = SimpleNote.note.GetDataSortByDate();

            SimpleNotes.Clear();
            foreach (var mood in moods)
                SimpleNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    public async void Receive(UpdateSimpleNoteMessage message)
    {
        await UpdateSimpleNote();
    }
}
