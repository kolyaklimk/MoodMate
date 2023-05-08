using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class MoodListViewModel : ObservableObject
{
    private readonly Note MoodNote;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();

    public MoodListViewModel(Note[] note)
    {
        MoodNote = note[0];
        IsRefreshing = false;

        MessagingCenter.Subscribe<CreateOrEditMoodViewModel>(this,
            "UpdateMoodNote", async (sender) => await UpdateMoodNote());
    }

    [ObservableProperty] bool isRefreshing;

    [RelayCommand]
    async void GoToMusicPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MusicListPage));
    }

    [RelayCommand]
    async void GoToNotePage()
    {
        await Shell.Current.GoToAsync("//" + nameof(NoteListPage));
    }

    [RelayCommand]
    async void GoToChooseMoodPage()
    {
        await Shell.Current.GoToAsync(nameof(ChooseMoodPage));
    }

    [RelayCommand]
    async void GoToAnalysisMoodPage()
    {
        await Shell.Current.GoToAsync(nameof(AnalysisMoodPage));
    }

    [RelayCommand]
    async Task UpdateMoodNote()
    {
        var moods = MoodNote.note.GetData();

        await Task.Run(() =>
        {
            MoodNotes.Clear();
            foreach (var mood in moods)
                MoodNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    async void Popup(MoodNote note)
    {
        var result = await Shell.Current.ShowPopupAsync(new ContextMenuPage());

        switch (result)
        {
            case 1:
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = "Date: " + note.Date.ToString() + '\n'
                    + "Mood: " + note.Mood.Name + '\n'
                    + "Description: " + note.Text,
                    Title = "Share Text"
                });
                break;

            case 2:
                await MoodNote.note.DeleteNote(note.Id);
                await UpdateMoodNote();
                break;

            case 3:
                await Shell.Current.GoToAsync(nameof(CreateOrEditMoodPage),
                    new Dictionary<string, object>() {
                    { "MoodNote", note}});
                break;
        }
    }
}
