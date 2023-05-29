using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Pages.Other;

namespace MoodMate.ViewModels.Other;

public partial class UserViewModel : ObservableObject
{
    private IUser User;
    public UserViewModel(IUser user)
    {
        User = user;
    }

    [ObservableProperty] string email;
    [ObservableProperty] string singInOut;
    [ObservableProperty] bool isLogIn;

    [RelayCommand]
    void Load()
    {
        Email = User.Client.User.Info.Email;

        if (User.Client.User != null)
        {
            IsLogIn = true;
            SingInOut = "Sign Out";
        }
        else
        {
            IsLogIn = false;
            SingInOut = "Sign In";
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
            await User.DeleteUser();
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
        }
        else
        {
            await Shell.Current.GoToAsync("//" + nameof(AuthenticationPage));
        }
    }
}
