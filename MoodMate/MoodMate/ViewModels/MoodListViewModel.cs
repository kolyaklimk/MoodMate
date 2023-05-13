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

public partial class MoodListViewModel : ObservableObject, IRecipient<UpdateMoodNoteMessage>
{
    private readonly Note MoodNote;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();

    public MoodListViewModel(Note[] note)
    {
        MoodNote = note[0];
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
    async Task GoToNotePage()
    {
        await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
    }

    [RelayCommand]
    async Task GoToChooseMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(ChooseMoodPage));
    }

    [RelayCommand]
    async Task GoToAnalysisMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(AnalysisMoodPage));
    }

    [RelayCommand]
    async Task UpdateMoodNote()
    {
        await Task.Run(() =>
        {
            var moods = MoodNote.note.GetData();

            MoodNotes.Clear();
            foreach (var mood in moods)
                MoodNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    async Task Popup(MoodNote note)
    {
        var result = await Shell.Current.ShowPopupAsync(new ContextMenuPage());

        switch (result)
        {
            case 1:
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = "Date: " + note.Date.ToString() + '\n'
                    + "Mood: " + note.Mood.Name + '\n'
                    + "Description: " + note.Text,
                    Title = "Share Text"
                });
                break;

            case 2:
                await MoodNote.note.DeleteNote(note.Id);
                await UpdateMoodNote();
                break;

            case 3:
                await Shell.Current.GoToAsync("//" + nameof(CreateOrEditMoodPage),
                    new Dictionary<string, object>() {
                        {"MoodNote", note},
                        {"Save", new MoodNote(note.Date, note.Mood.Name, note.Mood.Source, note.Text)}});
                break;
        }
    }

    public async void Receive(UpdateMoodNoteMessage message)
    {
        await UpdateMoodNote();
    }
}
