﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MoodMate.Components;
using MoodMate.Components.Entities;

namespace MoodMate;

public partial class App : Application
{
    public App(FileService[] fileServices, List<int>[] sbytes, IToast[] toasts)
    {
        InitializeComponent();
        LoadeService(fileServices, sbytes, toasts);
        MainPage = new AppShell();
    }
    private async void LoadeService(FileService[] fileServices, List<int>[] sbytes, IToast[] toasts)
    {
        await fileServices[0].LoadService(Constants.PathMoods);
        await fileServices[1].LoadService(Constants.PathMusic);
        await fileServices[2].LoadService(Constants.PathSound);

        sbytes[0] = Enumerable.Range(0, 24).ToList();
        sbytes[1] = Enumerable.Range(0, 60).ToList();

        toasts[0] = Toast.Make("Please choose mood!", ToastDuration.Short, 16);
        toasts[1] = Toast.Make("Please enter your mood!", ToastDuration.Short, 16);
        toasts[2] = Toast.Make("Please fill out a note!", ToastDuration.Short, 16);
        toasts[3] = Toast.Make("Please choose a sound or music!", ToastDuration.Short, 16);

        var localPath = Path.Combine(FileSystem.CacheDirectory, "admin.json");
        using var json = await FileSystem.OpenAppPackageFileAsync("admin.json");
        using (FileStream dest = File.Create(localPath))
            await json.CopyToAsync(dest);

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(localPath)
        });
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", localPath);
    }
}
