using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class NoteListViewModel : ObservableObject
{

    private readonly Note SmpleNote;
    public ObservableCollection<SimpleNote> SimpleNotes { get; set; } = new();

    public NoteListViewModel(Note[] note)
    {
        SmpleNote = note[1];

        MessagingCenter.Subscribe<CreateOrEditNoteViewModel>(this,
            "UpdateSimpleNote", (sender) => UpdateSimpleNote());
    }

    [RelayCommand]
    async void UpdateSimpleNote()
    {
        var moods = SmpleNote.note.GetData();
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            SimpleNotes.Clear();
            foreach (var mood in moods)
                SimpleNotes.Add(mood);
        });
    }

    [RelayCommand]
    async void Popup(MoodNote note)
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
                UpdateSimpleNote();
                break;

            case 3:
                await Shell.Current.GoToAsync(nameof(CreateOrEditNotePage),
                    new Dictionary<string, object>() {
                    { "SimpleNote", note}});
                break;
        }
    }
}
