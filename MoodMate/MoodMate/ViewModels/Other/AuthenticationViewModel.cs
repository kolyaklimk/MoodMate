using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Pages.MoodNote;

namespace MoodMate.ViewModels.Other;

public partial class AuthenticationViewModel: ObservableObject
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
        if(!IsRefreshing)
            await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
    }

    [RelayCommand]
    async Task SingIn()
    {
        if(!IsRefreshing)
        {
            IsRefreshing = true;

            if(await User.SingIn(Email, Password) == true)
            {
                await Shell.Current.GoToAsync("//" + nameof(MoodListPage));
            }
            else
            {
                Password = null;
            }

            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task SingUp()
    {
        if(!IsRefreshing)
        {
            IsRefreshing = true;

            if(await User.SingUp(Email, Password) == false)
                Password = null;

            IsRefreshing = false;
        }
    }
}
