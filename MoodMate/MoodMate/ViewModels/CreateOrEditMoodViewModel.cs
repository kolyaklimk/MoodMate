using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;

namespace MoodMate.ViewModels;

[QueryProperty(nameof(SelectedMood), "MoodNote")]
[QueryProperty(nameof(Create), "Create")]
public partial class CreateOrEditMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private readonly ChangeMoodMenuPage changeMoodMenuPage;
    public CreateOrEditMoodViewModel(Note[] note, ChangeMoodMenuPage page)
    {
        MoodNote = note[0];
        changeMoodMenuPage = page;
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
                SelectedMood.Date = SelectedMood.Date.Date.Add(DateTime.Now.TimeOfDay);
                MoodNote.note.AddNote(SelectedMood);
            }
            else
                MoodNote.note.ChangeNote(SelectedMood, SelectedMood.Id);

            MessagingCenter.Send(this, "UpdateMoodNote");
        }
    }

    [RelayCommand]
    async void ChooseImage()
    {
        var result = await Shell.Current.ShowPopupAsync(changeMoodMenuPage);
    }
}
