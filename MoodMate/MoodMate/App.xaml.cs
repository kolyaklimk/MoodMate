﻿using MoodMate.Components;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;

namespace MoodMate;

public partial class App : Application
{
    public App(Note[] notes, FileService[] fileServices, List<int>[] sbytes)
    {
        InitializeComponent();
        LoadeService(notes, fileServices, sbytes);
        MainPage = new AppShell();
    }
    private async void LoadeService(Note[] notes, FileService[] fileServices, List<int>[] sbytes)
    {
        await notes[0].note.LoadNote();
        await notes[1].note.LoadNote();
        await fileServices[0].LoadService(Constants.PathMoods);
        await fileServices[1].LoadService(Constants.PathMusic);
        await fileServices[2].LoadService(Constants.PathSound);
        sbytes[0] = await Task.Run(() => Enumerable.Range(0, 24).ToList());
        sbytes[1] = await Task.Run(() => Enumerable.Range(0, 60).ToList());
    }
}
