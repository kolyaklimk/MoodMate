using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Pages.MoodNote;

namespace MoodMate.ViewModels.Other;

public partial class AuthenticationViewModel : ObservableObject
{
    private readonly IUser User;

    [ObservableProperty] string email;
    [ObservableProperty] string password;
    [ObservableProperty] bool isRefreshing = false;
    public AuthenticationViewModel(IUser user)
    {
        User = user;
    }

    [RelayCommand]
    async Task Cancel()
    {
        if (!IsRefreshing)
        {
            if(User.Client.User!=null)
                User.Client.SignOut();
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
        }
    }

    [RelayCommand]
    async Task SingIn()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;

            if (await User.SingIn(Email, Password))
                await Shell.Current.GoToAsync("//" + nameof(MoodListPage));

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
