using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MoodMate.ViewModels;

public partial class MoodListViewModel : ObservableObject, IRecipient<UpdateMoodNoteMessage>
{
    private readonly Note MoodNote;
    private readonly IUser User;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();

    public MoodListViewModel(Note[] note, IUser user)
    {
        MoodNote = note[0];
        User = user;
        IsRefreshing = false;

        WeakReferenceMessenger.Default.Register(this);
        MoodNote.note.CreateDb();
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
    async Task GoToEdit(MoodNote note)
    {
        await Shell.Current.GoToAsync("//" + nameof(CreateOrEditMoodPage),
            new Dictionary<string, object>() {
                {"MoodNote", note},
                {"Save", new MoodNote(note.Date, note.Mood.Name, note.Mood.Source, note.Text)}});
    }

    [RelayCommand]
    void Load() => IsRefreshing = true;

    [RelayCommand]
    async Task RefreshMoodNote()
    {
        MoodNotes.Clear();
        await MoodNote.note.LoadNoteLocal();
        var moods = MoodNote.note.GetDataSortByDate() as List<MoodNote>;

        if (User.Client.User != null)
        {
            try
            {
                if (moods.Count() != 0)
                {
                    var rezult = await Shell.Current.ShowPopupAsync(new CloudWarningPage());
                    if ((bool)rezult)
                    {
                        await MoodNote.note.LoadNoteCloudAndSaveLocal(User.Client);
                    }
                    else
                    {
                        await MoodNote.note.LoadNoteCloud(User.Client);
                    }
                }
                else
                {
                    await MoodNote.note.LoadNoteCloud(User.Client);
                }
                moods = MoodNote.note.GetDataSortByDate();
            }
            catch
            {
                //await User.Alerts[2].Show();
            }
        }

        await Task.Run(() =>
        {

            foreach (var mood in moods)
                MoodNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    async Task UpdateMoodNote()
    {
        IsRefreshing = true;
        await Task.Run(() =>
        {
            var moods = MoodNote.note.GetDataSortByDate();

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
                try
                {
                    await MoodNote.note.DeleteNote(note.Id, User.Client);
                    await UpdateMoodNote();
                }
                catch
                {
                    await User.Alerts[2].Show();
                }
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
