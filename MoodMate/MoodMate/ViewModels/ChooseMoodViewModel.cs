﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities;
using MoodMate.Pages.MoodNote;
using System.Collections.ObjectModel;

namespace MoodMate.ViewModels;

public partial class ChooseMoodViewModel : ObservableObject
{
    private readonly FileService Mood;
    private IToast Alert;
    public ObservableCollection<FileService> Moods { get; set; } = new();

    [ObservableProperty] FileService selectedMood;
    [ObservableProperty] DateTime dateTime = DateTime.Now;

    public ChooseMoodViewModel(FileService[] service, IToast[] toasts)
    {
        Mood = service[0];
        Alert = toasts[0];
    }

    [RelayCommand]
    void SelectedItem(FileService mood)
    {
        if (SelectedMood == mood)
        {
            SelectedMood = null;
        }
        else
        {
            SelectedMood = mood;
        }
    }

    [RelayCommand]
    async Task LoadMoodNote()
    {
        await Task.Run(() =>
        {
            Moods.Clear();
            foreach (var mood in Mood.GetServiceData())
                Moods.Add(mood);
        });
    }

    [RelayCommand]
    async Task GoToCreateOrEditPage()
    {
        if (SelectedMood != null)
        {
            await Shell.Current.GoToAsync("//" + nameof(CreateOrEditMoodPage),
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
    public async Task Back_Clicked()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
        SelectedMood = null;
        DateTime = DateTime.Now;
    }
}
