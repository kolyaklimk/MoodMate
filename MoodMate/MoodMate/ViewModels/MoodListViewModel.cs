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

public partial class MoodListViewModel : ObservableObject, IRecipient<UpdateMoodNoteMessage>, IRecipient<LoadedMoodNoteMessage>
{
    private readonly MoodNote MoodNote;
    private readonly IUser User;
    private bool IsUpdating;
    private bool IsLoaded;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();
    [ObservableProperty] bool isRefreshing;
    [ObservableProperty] string icon;

    public MoodListViewModel(Note[] note, IUser user)
    {
        MoodNote = note[0].note;
        User = user;
        IsRefreshing = false;
        IsUpdating = false;
        IsLoaded = true;

        WeakReferenceMessenger.Default.Register<UpdateMoodNoteMessage>(this);
        WeakReferenceMessenger.Default.Register<LoadedMoodNoteMessage>(this);
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
    async Task GoToUserPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(UserPage), new Dictionary<string, object>() {
                {"Color", Color.FromArgb("#936840")},
                {"Page", nameof(MoodListPage)} });
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
    async Task UpdateMoodNote()
    {
        if (IsUpdating)
            return;

        if (IsLoaded)
            await MoodNote.LoadNoteLocal();

        List<MoodNote> moods;

        if (User.Client.User != null)
        {
            Icon = "icon_user_yes";

            if (IsLoaded)
            {
                IsLoaded = false;
            }
            else
            {
                MoodNote.ClearNotes();
            }
            moods = MoodNote.GetData();

            try
            {
                if (moods.Count != 0)
                {
                    var rezult = await Shell.Current.ShowPopupAsync(new CloudWarningPage());
                    switch (rezult)
                    {
                        case 1:
                            User.SignOut();
                            break;
                        case 2:
                            await MoodNote.DeleteNoteLocal();
                            await MoodNote.LoadNoteCloud(0, 15, User.Client.User);
                            break;
                        case 3:
                            await MoodNote.SaveLocalToCloud(User.Client.User);
                            await MoodNote.LoadNoteCloud(0, 15, User.Client.User);
                            break;
                    }
                }
                else
                {
                    await MoodNote.LoadNoteCloud(0, 15, User.Client.User);
                }
                moods = MoodNote.GetData();
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
            moods = MoodNote.GetDataSortByDate();
        }

        await Task.Run(() =>
        {
            MoodNotes.Clear();
            foreach (var mood in moods)
                MoodNotes.Add(mood);
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
                var countItemsBefore = MoodNote.GetData().Count;
                await MoodNote.LoadNoteCloud(countItemsBefore, 10, User.Client.User, false);
                var moods = MoodNote.GetData();

                for (var i = countItemsBefore; i < moods.Count; i++)
                {
                    MoodNotes.Add(moods[i]);
                }
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }
    }

    [RelayCommand]
    async Task ShareNote(MoodNote note)
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = "Date: " + note.Date.ToString() + '\n'
            + "Mood: " + note.Mood.Name + '\n'
            + "Description: " + note.Text,
            Title = "Share Text"
        });
    }

    [RelayCommand]
    async Task DeleteNote(MoodNote note)
    {
        IsUpdating = true;
        IsRefreshing = true;

        if (await Shell.Current.ShowPopupAsync(new ConfirmationPage("Delete selected record?", "Delete")) != null)
        {
            try
            {
                await MoodNote.DeleteNote(note, User.Client.User);
                MoodNotes.Remove(note);
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }
        IsRefreshing = false;
        IsUpdating = false;
    }

    void IRecipient<UpdateMoodNoteMessage>.Receive(UpdateMoodNoteMessage message) => IsRefreshing = true;

    void IRecipient<LoadedMoodNoteMessage>.Receive(LoadedMoodNoteMessage message)
    {
        if (!IsRefreshing)
        {
            IsLoaded = true;
            IsRefreshing = true;
        }
    }
}