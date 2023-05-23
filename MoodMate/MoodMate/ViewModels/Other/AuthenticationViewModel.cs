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
        var config = new FirebaseAuthConfig
        {
            ApiKey = "AIzaSyCt26JPyhXQM-XsLj0vNZN2tappF3Y17s4",
            AuthDomain = "moodmate-d9422.firebaseapp.com",
            Providers = new FirebaseAuthProvider[] { 
                new EmailProvider(),
                new GoogleProvider().AddScopes("email") 
            },
        };

        var client = new FirebaseAuthClient(config);
        UserCredential userCredential;
        string token;
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                userCredential = await client.SignInWithEmailAndPasswordAsync(Email, Password);
                Debug.WriteLine("Sing");
            }
            catch
            {
                try
                {
                    userCredential = await client.CreateUserWithEmailAndPasswordAsync(Email, Password);
                    Debug.WriteLine("Create");
                }
                catch
                {
                    Debug.WriteLine("Errur");
                }
            }
        }
        else
        {
            Debug.WriteLine("No Internet connection");
        }
    }
}
