using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Messages;

namespace MoodMate.ViewModels;


[QueryProperty(nameof(SelectedNote), "SimpleNote")]
[QueryProperty(nameof(Create), "Create")]
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

    [RelayCommand]
    async Task CreateOrEdit()
    {
        if (SelectedNote.Text != "" && SelectedNote.Text != null)
        {
            if (Create)
            {
                SelectedNote.Date = SelectedNote.Date.Date.Add(DateTime.Now.TimeOfDay);
                await SimpleNote.note.AddNote(SelectedNote);
            }
            else
                await SimpleNote.note.ChangeNote(SelectedNote, SelectedNote.Id);

            WeakReferenceMessenger.Default.Send(UpdateSimpleNoteMessage);
            await Shell.Current.Navigation.PopToRootAsync();
        }
        else
        {
            await Alert.Show();
        }
    }

    [RelayCommand]
    async void Back_Clicked()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
