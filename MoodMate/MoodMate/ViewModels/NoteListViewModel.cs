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

public partial class NoteListViewModel : ObservableObject, IRecipient<UpdateSimpleNoteMessage>, IRecipient<LoadedSimpleNoteMessage>
{

    private readonly SimpleNote SimpleNote;
    private readonly IUser User;
    private bool IsUpdating;
    private bool IsLoaded;
    public ObservableCollection<SimpleNote> SimpleNotes { get; set; } = new();
    [ObservableProperty] bool isRefreshing;
    [ObservableProperty] string icon;

    public NoteListViewModel(Note[] note, IUser user)
    {
        SimpleNote = note[1].note;
        User = user;
        IsRefreshing = false;
        IsUpdating = false;
        IsLoaded = true;

        WeakReferenceMessenger.Default.Register<UpdateSimpleNoteMessage>(this);
        WeakReferenceMessenger.Default.Register<LoadedSimpleNoteMessage>(this);
        SimpleNote.CreateDb();
    }

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
    async Task GoToUserPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(UserPage), new Dictionary<string, object>() {
                {"Color", Color.FromArgb("#2C3963")},
                {"Page", nameof(NoteListPage)}});
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
    void Load() => IsRefreshing = true;

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
        if (IsUpdating)
            return;

        if (IsLoaded)
            await SimpleNote.LoadNoteLocal();

        List<SimpleNote> notes;

        if (User.Client.User != null)
        {
            Icon = "icon_user_yes";

            if (IsLoaded)
            {
                IsLoaded = false;
            }
            else
            {
                SimpleNote.ClearNotes();
            }
            notes = SimpleNote.GetData();

            try
            {
                if (notes.Count != 0)
                {
                    var rezult = await Shell.Current.ShowPopupAsync(new CloudWarningPage());
                    switch (rezult)
                    {
                        case 1:
                            User.SignOut();
                            break;
                        case 2:
                            await SimpleNote.DeleteNoteLocal();
                            await SimpleNote.LoadNoteCloud(0, 15, User.Client.User);
                            break;
                        case 3:
                            await SimpleNote.SaveLocalToCloud(User.Client.User);
                            await SimpleNote.LoadNoteCloud(0, 15, User.Client.User);
                            break;
                    }
                }
                else
                {
                    await SimpleNote.LoadNoteCloud(0, 15, User.Client.User);
                }
                notes = SimpleNote.GetData();
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }
        else
        {
            if (IsLoaded)
                IsLoaded = false;

            Icon = "icon_user_no";
            notes = SimpleNote.GetDataSortByDate();
        }

        await Task.Run(() =>
        {
            SimpleNotes.Clear();
            foreach (var note in notes)
                SimpleNotes.Add(note);
        });
        IsRefreshing = false;
    }

    [RelayCommand]
    async Task AddItems()
    {
        if (User.Client.User != null && !IsRefreshing)
        {
            try
            {
                var countItemsBefore = SimpleNote.GetData().Count;
                await SimpleNote.LoadNoteCloud(countItemsBefore, 10, User.Client.User, false);
                var notes = SimpleNote.GetData();

                for (var i = countItemsBefore; i < notes.Count; i++)
                {
                    SimpleNotes.Add(notes[i]);
                }
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }
    }

    [RelayCommand]
    async Task ShareNote(SimpleNote note)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = "Date: " + note.Date.ToString() + '\n'
            + "Text: " + note.Text,
            Title = "Share Text"
        });
    }

    [RelayCommand]
    async Task DeleteNote(SimpleNote note)
    {
        IsUpdating = true;
        IsRefreshing = true;

        if (await Shell.Current.ShowPopupAsync(new ConfirmationPage("Delete selected record?", "Delete")) != null)
        {
            try
            {
                await SimpleNote.DeleteNote(note, User.Client.User);
                SimpleNotes.Remove(note);
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }
        IsRefreshing = false;
        IsUpdating = false;
    }

    void IRecipient<UpdateSimpleNoteMessage>.Receive(UpdateSimpleNoteMessage message) => IsRefreshing = true;

    void IRecipient<LoadedSimpleNoteMessage>.Receive(LoadedSimpleNoteMessage message)
    {
        if (!IsRefreshing)
        {
            IsLoaded = true;
            IsRefreshing = true;
        }
    }
}
