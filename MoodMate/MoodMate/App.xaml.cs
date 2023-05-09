using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Animations;
using MoodMate.Components;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;

namespace MoodMate;

public partial class App : Application
{
    public App(Note[] notes, FileService[] fileServices, List<int>[] sbytes, IToast[] toasts)
    {
        InitializeComponent();
        LoadeService(notes, fileServices, sbytes, toasts);
        MainPage = new AppShell();
    }
    private async void LoadeService(Note[] notes, FileService[] fileServices, List<int>[] sbytes, IToast[] toasts)
    {
        notes[0].note.LoadNote();
        notes[1].note.LoadNote();
        fileServices[0].LoadService(Constants.PathMoods);
        fileServices[1].LoadService(Constants.PathMusic);
        fileServices[2].LoadService(Constants.PathSound);
        sbytes[0] = await Task.Run(() => Enumerable.Range(0, 24).ToList());
        sbytes[1] = await Task.Run(() => Enumerable.Range(0, 60).ToList());

        Task.Run(() =>
        {
            toasts[0] = Toast.Make("Please choose mood!", ToastDuration.Short, 16);
            toasts[1] = Toast.Make("Please enter your mood!", ToastDuration.Short, 16);
            toasts[2] = Toast.Make("Please fill out a note!", ToastDuration.Short, 16);
            toasts[3] = Toast.Make("Please choose a sound or music!", ToastDuration.Short, 16);
        });
    }
}
