using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;

namespace MoodMate.ViewModels;


[QueryProperty(nameof(SelectedNote), "SimpleNote")]
[QueryProperty(nameof(Create), "Create")]
public partial class CreateOrEditNoteViewModel : ObservableObject
{
    private readonly Note SimpleNote;
    public CreateOrEditNoteViewModel(Note[] note)
    {
        SimpleNote = note[1];
    }

    [ObservableProperty] private bool create;
    [ObservableProperty] SimpleNote selectedNote;

    [RelayCommand]
    async Task CreateOrEdit()
    {
        if (SelectedNote.Text != "")
        {
            if (Create)
            {
                SelectedNote.Date = SelectedNote.Date.Date.Add(DateTime.Now.TimeOfDay);
                await SimpleNote.note.AddNote(SelectedNote);
            }
            else
                await SimpleNote.note.ChangeNote(SelectedNote, SelectedNote.Id);

            MessagingCenter.Send(this, "UpdateSimpleNote");
            await Shell.Current.Navigation.PopToRootAsync();
        }
    }

    [RelayCommand]
    async void Back_Clicked()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
