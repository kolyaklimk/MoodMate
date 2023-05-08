﻿using CommunityToolkit.Maui.Views;
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

public partial class NoteListViewModel : ObservableObject
{

    private readonly Note SmpleNote;
    public ObservableCollection<SimpleNote> SimpleNotes { get; set; } = new();

    public NoteListViewModel(Note[] note)
    {
        SmpleNote = note[1];
        IsRefreshing = false;

        MessagingCenter.Subscribe<CreateOrEditNoteViewModel>(this,
            "UpdateSimpleNote", async (sender) => await UpdateSimpleNote());
    }

    [ObservableProperty] bool isRefreshing;

    [RelayCommand]
    async void GoToMusicPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MusicListPage));
    }

    [RelayCommand]
    async void GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
    }

    [RelayCommand]
    async void GoToCreateOrEditPage()
    {
        await Shell.Current.GoToAsync(nameof(CreateOrEditNotePage),
                new Dictionary<string, object>() {
                    { "SimpleNote", new SimpleNote() { Date = DateTime.Now }},
                    { "Create", true}});
    }

    [RelayCommand]
    async Task UpdateSimpleNote()
    {
        var moods = SmpleNote.note.GetData();

        await Task.Run(() =>
        {
            SimpleNotes.Clear();
            foreach (var mood in moods)
                SimpleNotes.Add(mood);
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    public async void Popup(SimpleNote note)
    {
        var result = await Shell.Current.ShowPopupAsync(new ContextMenuPage());

        switch (result)
        {
            case 1:
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = "Date: " + note.Date.ToString() + '\n'
                    + "Text: " + note.Text
                });
                break;

            case 2:
                await SmpleNote.note.DeleteNote(note.Id);
                await UpdateSimpleNote();
                break;

            case 3:
                await Shell.Current.GoToAsync(nameof(CreateOrEditNotePage),
                    new Dictionary<string, object>() {
                    { "SimpleNote", note}});
                break;
        }
    }
}
