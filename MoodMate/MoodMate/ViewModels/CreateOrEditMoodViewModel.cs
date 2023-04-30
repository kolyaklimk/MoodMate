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
    private readonly bool Create;

    public CreateOrEditMoodViewModel(Note[] note)
    {
        MoodNote = note[0];
    }
    [ObservableProperty] public MoodNote selectedMood;

    [RelayCommand]
    void CreateOrEdit()
    {
        if (SelectedMood.Mood.Name != "")
        {
            if (Create)
                MoodNote.note.AddNote(SelectedMood);
            else
                MoodNote.note.ChangeNote(SelectedMood, SelectedMood.Id);
        }
    }
}
