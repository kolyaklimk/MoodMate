using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Other;

namespace MoodMate.ViewModels.Other;

public partial class UserViewModel : ObservableObject
{
    private IUser User;
    private readonly LoadedMoodNoteMessage LoadedMoodNoteMessage;
    public UserViewModel(IUser user, LoadedMoodNoteMessage loaded)
    {
        User = user;
        LoadedMoodNoteMessage = loaded;
    }

    [ObservableProperty] string email;
    [ObservableProperty] string singInOut;
    [ObservableProperty] string label;
    [ObservableProperty] bool isLogIn;

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
        await User.SendEmailPasswordResetLink();
    }

    [RelayCommand]
    async Task DeleteUser()
    {
        if (await Shell.Current.ShowPopupAsync(new ConfirmationPage("Delete the account?")) != null)
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
    }

    [RelayCommand]
    async Task SignInOut()
    {
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
    }

    [RelayCommand]
    public async Task GoToMoodPage()
    {
        await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
    }
}
