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

namespace MoodMate.ViewModels;

public partial class MoodListViewModel : ObservableObject, IRecipient<UpdateMoodNoteMessage>
{
    private readonly MoodNote MoodNote;
    private readonly IUser User;
    private bool IsUpdating;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();
    [ObservableProperty] bool isRefreshing;

    public MoodListViewModel(Note[] note, IUser user)
    {
        MoodNote = note[0].note;
        User = user;
        IsRefreshing = false;
        IsUpdating = false;

        WeakReferenceMessenger.Default.Register(this);
        MoodNote.CreateDb();
    }


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
        if (IsUpdating)
            return;

        await MoodNote.LoadNoteLocal();
        var moods = MoodNote.GetData();

        if (User.Client.User != null)
        {
            try
            {
                if (moods.Count() != 0)
                {
                    var rezult = await Shell.Current.ShowPopupAsync(new CloudWarningPage());
                    switch (rezult)
                    {
                        case 1:
                            User.Client.SignOut();
                            break;
                        case 2:
                            await MoodNote.LoadNoteCloud(User.Client.User);
                            break;
                        case 3:
                            await MoodNote.SaveLocalToCloud(User.Client.User);
                            await MoodNote.LoadNoteCloud(User.Client.User);
                            break;
                    }
                }
                else
                {
                    await MoodNote.LoadNoteCloud(User.Client.User);
                }
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }

        await Task.Run(() =>
        {
            MoodNotes.Clear();
            foreach (var mood in MoodNote.GetDataSortByDate())
                MoodNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    async Task UpdateMoodNote()
    {
        await Task.Run(() =>
        {
            IsUpdating = true;
            IsRefreshing = true;

            MoodNotes.Clear();
            foreach (var mood in MoodNote.GetDataSortByDate())
                MoodNotes.Add(mood);

            IsRefreshing = false;
            IsUpdating = false;
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
                    await MoodNote.DeleteNote(note, User.Client.User);
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
