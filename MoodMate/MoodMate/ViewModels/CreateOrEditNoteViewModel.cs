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
    private readonly UpdateSimpleNoteMessage UpdateSimpleNoteMessage = new();
    private readonly IUser User;
    private IToast Alert;

    [ObservableProperty] private bool create;
    [ObservableProperty] SimpleNote selectedNote;
    [ObservableProperty] private SimpleNote savededNote;
    [ObservableProperty] bool isRefreshing = false;

    public CreateOrEditNoteViewModel(Note[] note, IToast[] toasts, IUser user)
    {
        SimpleNote = note[1].note;
        Alert = toasts[2];
        User = user;
    }

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
                WeakReferenceMessenger.Default.Send(UpdateSimpleNoteMessage);
                await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
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
