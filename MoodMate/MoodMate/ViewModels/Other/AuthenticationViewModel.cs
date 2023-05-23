using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;
using System.Diagnostics;

namespace MoodMate.ViewModels.Other;

public partial class AuthenticationViewModel : ObservableObject
{
    [ObservableProperty] string email;
    [ObservableProperty] string password;


    [RelayCommand]
    async Task ClickButton()
    {
        try
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyCt26JPyhXQM-XsLj0vNZN2tappF3Y17s4\r\n",
                AuthDomain = "moodmate-d9422-default-rtdb.firebaseapp.com",
                Providers = new FirebaseAuthProvider[] { new EmailProvider() },
            };

            var client = new FirebaseAuthClient(config);
            var userCredential = await client.SignInWithEmailAndPasswordAsync(Email, Password);
            if (userCredential == null)
            {
                Debug.WriteLine("create");
                userCredential = await client.CreateUserWithEmailAndPasswordAsync(Email, Password);
            }
            Debug.WriteLine("sing");
        }
        catch (Exception ex)
        {
        }
    }
}
