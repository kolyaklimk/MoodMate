﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Other;

namespace MoodMate.ViewModels.Other;

[QueryProperty(nameof(Color), "Color")]
public partial class UserViewModel : ObservableObject
{
    private IUser User;
    private readonly LoadedMoodNoteMessage LoadedMoodNoteMessage;
    public UserViewModel(IUser user, LoadedMoodNoteMessage loaded)
    {
        User = user;
        LoadedMoodNoteMessage = loaded;
        IsRefreshing = false;
    }

    [ObservableProperty] string email;
    [ObservableProperty] string singInOut;
    [ObservableProperty] string label;
    [ObservableProperty] Color color;
    [ObservableProperty] bool isLogIn;
    [ObservableProperty] bool isRefreshing;

    [RelayCommand]
    void Load()
    {

        if (User.Client.User != null)
        {
            Email = User.Client.User.Info.Email;
            IsLogIn = true;
            SingInOut = "Sign Out";
            Label = "Email:";
        }
        else
        {
            IsLogIn = false;
            SingInOut = "Sign In";
            Label = "Offline";
        }
    }

    [RelayCommand]
    async Task ResetPassword()
    {
        IsRefreshing = true;
        await User.SendEmailPasswordResetLink(User.Client.User.Info.Email);
        IsRefreshing = false;
    }

    [RelayCommand]
    async Task DeleteUser()
    {
        IsRefreshing = true;
        if (await Shell.Current.ShowPopupAsync(new ConfirmationPage("Delete the account?", "Delete")) != null)
        {
            try
            {
                await User.DeleteUser();
                await Shell.Current.GoToAsync("//" + nameof(AuthenticationPage));
            }
            catch
            {
                await User.Alerts[2].Show();
            }
        }
        IsRefreshing = false;
    }

    [RelayCommand]
    async Task SignInOut()
    {
        IsRefreshing = true;
        if (IsLogIn)
        {
            User.SignOut();
            IsLogIn = false;
            SingInOut = "Sign In";
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
            WeakReferenceMessenger.Default.Send(LoadedMoodNoteMessage);
        }
        else
        {
            await Shell.Current.GoToAsync("//" + nameof(AuthenticationPage));
        }
        IsRefreshing = false;
    }

    [RelayCommand]
    public async Task GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
    }
}