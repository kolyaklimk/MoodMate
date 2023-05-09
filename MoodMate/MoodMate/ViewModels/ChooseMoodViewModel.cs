using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class ChooseMoodViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private readonly FileService Mood;
    private IToast Alert;
    public ChooseMoodViewModel(Note[] note, FileService[] service, IToast[] toasts)
    {
        MoodNote = note[0];
        Mood = service[0];
        Alert = toasts[0];
    }
    public ObservableCollection<FileService> Moods { get; set; } = new();

    [ObservableProperty] FileService selectedMood;
    [ObservableProperty] DateTime dateTime = DateTime.Now;

    [RelayCommand]
    async Task LoadMoodNote()
    {
        await Task.Run(() =>
        {
            var moods = Mood.GetServiceData();

            Moods.Clear();
            foreach (var mood in moods)
                Moods.Add(mood);
        });
    }

    [RelayCommand]
    async void GoToCreateOrEditPage()
    {
        if (SelectedMood != null)
        {
            await Shell.Current.GoToAsync(nameof(CreateOrEditMoodPage),
                new Dictionary<string, object>() {
                    { "MoodNote", new MoodNote() {
                        Mood = new FileService(){ Name=SelectedMood.Name, Source=SelectedMood.Source},
                        Date = DateTime }},
                    { "Create", true}});
            SelectedMood = null;
            DateTime = DateTime.Now;
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
