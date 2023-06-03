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
    private readonly LoadedSimpleNoteMessage LoadedSimpleNoteMessage;

    [ObservableProperty] string email;
    [ObservableProperty] string password;
    [ObservableProperty] bool isRefreshing = false;

    public AuthenticationViewModel(IUser user, LoadedMoodNoteMessage loaded1, LoadedSimpleNoteMessage loaded2)
    {
        User = user;
        LoadedMoodNoteMessage = loaded1;
        LoadedSimpleNoteMessage = loaded2;
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
    async Task Offline()
    {
        if (!IsRefreshing)
        {
            if (User.Client.User != null)
                User.SignOut();
            WeakReferenceMessenger.Default.Send(LoadedMoodNoteMessage);
            WeakReferenceMessenger.Default.Send(LoadedSimpleNoteMessage);
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
        }
    }

    [RelayCommand]
    async Task SingIn()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;
            if (await User.SingIn(Email, Password) == 1)
            {
                await User.SaveEmailAndPasswordLocal(Email, Password);
                WeakReferenceMessenger.Default.Send(LoadedMoodNoteMessage);
                WeakReferenceMessenger.Default.Send(LoadedSimpleNoteMessage);
                await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
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
