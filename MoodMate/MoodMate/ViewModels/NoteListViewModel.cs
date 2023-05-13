using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class NoteListViewModel : ObservableObject, IRecipient<UpdateSimpleNoteMessage>
{

    private readonly Note SmpleNote;
    public ObservableCollection<SimpleNote> SimpleNotes { get; set; } = new();

    public NoteListViewModel(Note[] note)
    {
        SmpleNote = note[1];
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
    async Task GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
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
            var moods = SmpleNote.note.GetData();

            SimpleNotes.Clear();
            foreach (var mood in moods)
                SimpleNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    public async Task Popup(SimpleNote note)
    {
        var result = await Shell.Current.ShowPopupAsync(new ContextMenuPage());

        switch (result)
        {
            case 1:
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = "Date: " + note.Date.ToString() + '\n'
                    + "Text: " + note.Text
                });
                break;

            case 2:
                await SmpleNote.note.DeleteNote(note.Id);
                await UpdateSimpleNote();
                break;

            case 3:
                await Shell.Current.GoToAsync("//" + nameof(CreateOrEditNotePage),
                    new Dictionary<string, object>() {
                        {"SimpleNote", note},
                        {"Save", new SimpleNote(note.Date, note.Text)}});
                break;
        }
    }

    public async void Receive(UpdateSimpleNoteMessage message)
    {
        await UpdateSimpleNote();
    }
}
