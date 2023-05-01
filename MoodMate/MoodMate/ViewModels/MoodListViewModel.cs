using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;
using System.Collections.ObjectModel;
using MoodMate.Pages.Other;

namespace MoodMate.ViewModels;

public partial class MoodListViewModel : ObservableObject
{
    private readonly Note MoodNote;
    private readonly ContextMenuPage contextMenuPage;
    public ObservableCollection<MoodNote> MoodNotes { get; set; } = new();

    public MoodListViewModel(Note[] note, ContextMenuPage page)
    {
        MoodNote = note[0];
        contextMenuPage = page;

        MessagingCenter.Subscribe<CreateOrEditMoodViewModel>(this,
            "UpdateMoodNote", (sender) => UpdateMoodNote());
    }

    [RelayCommand]
    async void GoToChooseMoodPage()
    {
        await Shell.Current.GoToAsync(nameof(ChooseMoodPage));
    }

    [RelayCommand]
    async void UpdateMoodNote()
    {
        var moods = MoodNote.note.GetData();
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            MoodNotes.Clear();
            foreach (var mood in moods)
                MoodNotes.Add(mood);
        });
    }

    [RelayCommand]
    async void Popup(MoodNote note)
    {
        var result = await Shell.Current.ShowPopupAsync(contextMenuPage);

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
                UpdateMoodNote();
                break;

            case 3:
                await Shell.Current.GoToAsync(nameof(CreateOrEditMoodPage),
                    new Dictionary<string, object>() {
                    { "MoodNote", note}});
                break;
        }
    }
}
