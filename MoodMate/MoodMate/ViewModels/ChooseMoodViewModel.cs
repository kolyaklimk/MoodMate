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
    public ChooseMoodViewModel(Note[] note, FileService[] service)
    {
        MoodNote = note[0];
        Mood = service[0];
    }
    public ObservableCollection<FileService> Moods { get; set; } = new();

    [ObservableProperty] FileService selectedMood;
    [ObservableProperty] DateTime dateTime = DateTime.Now;

    [RelayCommand]
    void LoadMoodNote()
    {
        var moods = Mood.GetServiceData();

        Moods.Clear();
        foreach (var mood in moods)
            Moods.Add(mood);
    }

    [RelayCommand]
    async void GoToCreateOrEditPage()
    {
        OnPropertyChanged(nameof(MoodNote));
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
    }
    
    [RelayCommand]
    async void Back_Clicked()
    {
        await Shell.Current.Navigation.PopAsync();
    }
}
