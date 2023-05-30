using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Other;

namespace MoodMate.ViewModels;

[QueryProperty(nameof(SelectedMood), "MoodNote")]
[QueryProperty(nameof(Create), "Create")]
[QueryProperty(nameof(SavededMood), "Save")]
public partial class CreateOrEditMoodViewModel : ObservableObject
{
    private readonly MoodNote MoodNote;
    private readonly FileService MoodImages;
    private readonly UpdateMoodNoteMessage UpdateMoodNoteMessage = new();
    private IToast Alert;
    private readonly IUser User;

    [ObservableProperty] private bool create;
    [ObservableProperty] public MoodNote selectedMood;
    [ObservableProperty] private MoodNote savededMood;
    [ObservableProperty] bool isRefreshing = false;

    public CreateOrEditMoodViewModel(Note[] note, FileService[] fileService,
        IToast[] toasts, IUser user)
    {
        MoodNote = note[0].note;
        MoodImages = fileService[0];
        Alert = toasts[1];
        User = user;
    }

    [RelayCommand]
    async Task CreateOrEdit()
    {
        IsRefreshing = true;
        if (SelectedMood.Mood.Name != "")
        {
            try
            {
                if (Create)
                {
                    SelectedMood.Date = SelectedMood.Date.Date.Add(DateTime.Now.TimeOfDay);
                    await MoodNote.AddNote(SelectedMood, User.Client.User);
                }
                else
                {
                    await MoodNote.ChangeNote(SelectedMood, User.Client.User);
                }
                await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
                WeakReferenceMessenger.Default.Send(UpdateMoodNoteMessage);
            }
            catch
            {
                if ((bool)await Shell.Current.ShowPopupAsync(new GoOfflinePage(" You can go offline to save your data.")))
                {
                    User.SignOut();
                    Create = true;
                    MoodNote.ClearNotes();
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
    async Task ChooseImage()
    {
        var result = await Shell.Current.ShowPopupAsync(new ChangeMoodMenuPage(MoodImages)) as Tuple<string, string>;
        if (result != null)
        {
            SelectedMood.Mood.Source = result.Item1;
            SelectedMood.Mood.Name = result.Item2;
        }
    }

    [RelayCommand]
    public async Task Back_Clicked()
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
