using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;

namespace MoodMate.ViewModels;

[QueryProperty(nameof(SelectedMood), "MoodNote")]
[QueryProperty(nameof(Create), "Create")]
[QueryProperty(nameof(SavededMood), "Save")]
public partial class CreateOrEditMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private readonly FileService MoodImages;
    private readonly UpdateMoodNoteMessage UpdateMoodNoteMessage;
    private IToast Alert;
    public CreateOrEditMoodViewModel(Note[] note, FileService[] fileService, UpdateMoodNoteMessage update, IToast[] toasts)
    {
        MoodNote = note[0];
        MoodImages = fileService[0];
        UpdateMoodNoteMessage = update;
        Alert = toasts[1];
    }

    [ObservableProperty] private bool create;
    [ObservableProperty] public MoodNote selectedMood;
    [ObservableProperty] private MoodNote savededMood;

    [RelayCommand]
    async Task CreateOrEdit()
    {
        if (SelectedMood.Mood.Name != "")
        {
            if (Create)
            {
                SelectedMood.Date = SelectedMood.Date.Date.Add(DateTime.Now.TimeOfDay);
                await MoodNote.note.AddNote(SelectedMood);
            }
            else
                await MoodNote.note.ChangeNote(SelectedMood, SelectedMood.Id);

            WeakReferenceMessenger.Default.Send(UpdateMoodNoteMessage);
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
        }
        else
        {
            await Alert.Show();
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
        }
    }

    [RelayCommand]
    async void Back_Clicked()
    {
        if (Create)
        {
            await Shell.Current.GoToAsync("//" + nameof(ChooseMoodPage));
        }
        else
        {
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
            SelectedMood.Mood.Source = SavededMood.Mood.Source;
            SelectedMood.Mood.Name = SavededMood.Mood.Name;
            SelectedMood.Text = SavededMood.Text;
            SelectedMood.Date = SavededMood.Date;
        }
    }
}
