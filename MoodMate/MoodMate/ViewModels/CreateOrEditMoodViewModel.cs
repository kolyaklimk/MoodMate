using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;

namespace MoodMate.ViewModels;

[QueryProperty(nameof(SelectedMood), "MoodNote")]
[QueryProperty(nameof(Create), "Create")]
public partial class CreateOrEditMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    public CreateOrEditMoodViewModel(Note[] note)
    {
        MoodNote = note[0];
    }

    [ObservableProperty] private bool create;
    [ObservableProperty] public MoodNote selectedMood;

    [RelayCommand]
    void CreateOrEdit()
    {
        if (SelectedMood.Mood.Name != "")
        {
            if (Create)
            {
                SelectedMood.Date = SelectedMood.Date.Date.Add(SelectedMood.Date.TimeOfDay);
                MoodNote.note.AddNote(SelectedMood);
            }
            else
                MoodNote.note.ChangeNote(SelectedMood, SelectedMood.Id);

            MessagingCenter.Send(this, "UpdateMoodNote");
        }
    }
}
