using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;

namespace MoodMate.ViewModels.Other;

public partial class AuthenticationViewModel : ObservableObject
{
    private readonly IUser User;
    private readonly LoadedMoodNoteMessage LoadedMoodNoteMessage;

    [ObservableProperty] string email;
    [ObservableProperty] string password;
    [ObservableProperty] bool isRefreshing;
    public AuthenticationViewModel(IUser user, LoadedMoodNoteMessage loaded)
    {
        User = user;
        IsRefreshing = false;
        LoadedMoodNoteMessage = loaded;
    }

    [RelayCommand]
    async Task Load()
    {
        try
        {
            var emailAndPassword = await User.LoadEmailAndPasswordLocal();
            Email = emailAndPassword.Email;
            Password = emailAndPassword.Password;
        }
        catch { }
    }

    [RelayCommand]
    async Task Cancel()
    {
        if (!IsRefreshing)
        {
            if (User.Client.User != null)
                User.SignOut();
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
            WeakReferenceMessenger.Default.Send(LoadedMoodNoteMessage);
        }
    }

    [RelayCommand]
    async Task SingIn()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;

            if (await User.SingIn(Email, Password))
            {
                await User.SaveEmailAndPasswordLocal(Email, Password);
                await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
                WeakReferenceMessenger.Default.Send(LoadedMoodNoteMessage);
            }

            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task SingUp()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;

            await User.SingUp(Email, Password);

            IsRefreshing = false;
        }
    }
}
