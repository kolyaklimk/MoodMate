using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.SimpleNote;

namespace MoodMate.ViewModels;


[QueryProperty(nameof(SelectedNote), "SimpleNote")]
[QueryProperty(nameof(Create), "Create")]
[QueryProperty(nameof(SavededNote), "Save")]
public partial class CreateOrEditNoteViewModel : ObservableObject
{
    private readonly Note SimpleNote;
    private readonly UpdateSimpleNoteMessage UpdateSimpleNoteMessage;
    private IToast Alert;
    public CreateOrEditNoteViewModel(Note[] note, UpdateSimpleNoteMessage update, IToast[] toasts)
    {
        SimpleNote = note[1];
        UpdateSimpleNoteMessage = update;
        Alert = toasts[2];
    }

    [ObservableProperty] private bool create;
    [ObservableProperty] SimpleNote selectedNote;
    [ObservableProperty] private SimpleNote savededNote;

    [RelayCommand]
    async Task CreateOrEdit()
    {
        if (SelectedNote.Text != "" && SelectedNote.Text != null)
        {
            SelectedNote.Date = DateTime.SpecifyKind(SelectedNote.Date.Date.Add(DateTime.Now.TimeOfDay), DateTimeKind.Utc);
            if (Create)
            {
                await SimpleNote.note.AddNote(SelectedNote);
            }
            else
                await SimpleNote.note.ChangeNote(SelectedNote, SelectedNote.Id);

            WeakReferenceMessenger.Default.Send(UpdateSimpleNoteMessage);
            await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
        }
        else
        {
            await Alert.Show();
        }
    }

    [RelayCommand]
    public async Task ShareItem()
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = "Date: " + SelectedNote.Date.ToString() + '\n'
                    + "Text: " + SelectedNote.Text
        });
    }

    [RelayCommand]
    public async Task DeleteItem()
    {
        await SimpleNote.note.DeleteNote(SelectedNote.Id);
        WeakReferenceMessenger.Default.Send(UpdateSimpleNoteMessage);
        await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
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
