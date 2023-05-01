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
    private readonly FileService MoodImages;
    public CreateOrEditMoodViewModel(Note[] note, FileService[] fileService)
    {
        MoodNote = note[0];
        MoodImages = fileService[0];
    }

    [ObservableProperty] private bool create;
    [ObservableProperty] public MoodNote selectedMood;

    [RelayCommand]
    async void CreateOrEdit()
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
            await Shell.Current.Navigation.PopToRootAsync();
        }
    }

    [RelayCommand]
    async void ChooseImage()
    {
        var result = await Shell.Current.ShowPopupAsync(new ChangeMoodMenuPage(MoodImages)) as Tuple<string, string>;
        if (result != null)
        {
            SelectedMood.Mood.Source = result.Item1;
            SelectedMood.Mood.Name = result.Item2;
            OnPropertyChanged(nameof(SelectedMood));
        }
    }

    [RelayCommand]
    async void Back_Clicked()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
