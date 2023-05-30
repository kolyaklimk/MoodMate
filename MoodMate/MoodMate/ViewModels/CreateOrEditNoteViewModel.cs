using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;

namespace MoodMate.ViewModels;


[QueryProperty(nameof(SelectedNote), "SimpleNote")]
[QueryProperty(nameof(Create), "Create")]
[QueryProperty(nameof(SavededNote), "Save")]
public partial class CreateOrEditNoteViewModel : ObservableObject
{
    private readonly SimpleNote SimpleNote;
    private readonly UpdateSimpleNoteMessage UpdateSimpleNoteMessage;
    private IToast Alert;
    private readonly IUser User;
    public CreateOrEditNoteViewModel(Note[] note, IToast[] toasts,
        UpdateSimpleNoteMessage update, IUser user)
    {
        SimpleNote = note[1].note;
        UpdateSimpleNoteMessage = update;
        Alert = toasts[2];
        User = user;
        IsRefreshing = false;
    }

    [ObservableProperty] private bool create;
    [ObservableProperty] SimpleNote selectedNote;
    [ObservableProperty] private SimpleNote savededNote;
    [ObservableProperty] bool isRefreshing;

    [RelayCommand]
    async Task CreateOrEdit()
    {
        IsRefreshing = true;

        if (SelectedNote.Text != "" && SelectedNote.Text != null)
        {
            try
            {
                SelectedNote.Date = DateTime.Now;
                if (Create)
                {
                    await SimpleNote.AddNote(SelectedNote, User.Client.User);
                }
                else
                {
                    await SimpleNote.ChangeNote(SelectedNote, User.Client.User);
                }

                await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
                WeakReferenceMessenger.Default.Send(UpdateSimpleNoteMessage);
            }
            catch
            {
                if ((bool)await Shell.Current.ShowPopupAsync(new GoOfflinePage(" You can go offline to save your data.")))
                {
                    User.SignOut();
                    Create = true;
                    SimpleNote.ClearNotes();
                    await CreateOrEdit();
                }
            }
        }
        else
        {
            await Alert.Show();
        }

        IsRefreshing = false;
    }

    [RelayCommand]
    public async Task Back_Clicked()
    {
        await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
        if (!Create)
        {
            SelectedNote.Text = SavededNote.Text;
            SelectedNote.Date = SavededNote.Date;
        }
    }
}
