using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using MailKit.Net.Smtp;
using MimeKit;
using MoodMate.Components;
using System.Diagnostics;

namespace MoodMate.ViewModels.Other;

public partial class AuthenticationViewModel : ObservableObject
{
    private FirebaseAuthClient client;
    private MimeMessage EmailMessage;

    [ObservableProperty] string email;
    [ObservableProperty] string password;
    [ObservableProperty] bool isRefreshing = false;
    public AuthenticationViewModel()
    {
        client = new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = PrivateConstants.ApiKey,
            AuthDomain = PrivateConstants.AuthDomain,
            Providers = new FirebaseAuthProvider[] { new EmailProvider(), },
        });

        EmailMessage = new MimeMessage();
        EmailMessage.From.Add(new MailboxAddress("MoodMate", PrivateConstants.Email));
        EmailMessage.Subject = "Verify your email address";
    }

    [RelayCommand]
    async Task Cancel()
    {
    }

    [RelayCommand]
    async Task ClickButton()
    {
        IsRefreshing = true;
        UserCredential userCredential;
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                userCredential = await client.SignInWithEmailAndPasswordAsync(Email, Password);
                if (!userCredential.User.Info.IsEmailVerified)
                {
                    var link = await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(Email);

                    await SendEmail(Email, link);
                    Debug.WriteLine("Email not Verified");
                    userCredential = null;
                }
                else
                {

                    Debug.WriteLine("Sing");
                }
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

        IsRefreshing = false;
    }

    private async Task SendEmail(string email, string link)
    {
        EmailMessage.To.Add(new MailboxAddress("", email));
        EmailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = "<h2>" + "<a href=" + link + " >Сlick here</a>" + " to verify your email address.</h2><br>"
        };

        using (var smtpClient = new SmtpClient())
        {
            try
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtpClient.Connect("smtp.yandex.ru", 465, true);
                smtpClient.Authenticate(PrivateConstants.Email, PrivateConstants.EmailPassword);
                smtpClient.Send(EmailMessage);
                smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
