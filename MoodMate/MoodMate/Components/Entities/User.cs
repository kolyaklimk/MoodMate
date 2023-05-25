using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using MailKit.Net.Smtp;
using MimeKit;
using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Entities;

public class User: IUser
{
    public FirebaseAuthClient Client { get; set; }
    public MimeMessage EmailMessage { get; set; }
    public IToast[] Alerts { get; set; }

    public User()
    {
        Client = new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = PrivateConstants.ApiKey,
            AuthDomain = PrivateConstants.AuthDomain,
            Providers = new FirebaseAuthProvider[] { new EmailProvider(), },
        });

        EmailMessage = new MimeMessage();
        EmailMessage.From.Add(new MailboxAddress("MoodMate", PrivateConstants.Email));
        EmailMessage.Subject = "Verify your email address";

        Alerts = new IToast[3];
        Alerts[0] = Toast.Make("Check your email for verification!", ToastDuration.Short, 16);
        Alerts[1] = Toast.Make("Wrong email or password!", ToastDuration.Short, 16);
        Alerts[2] = Toast.Make("No internet connection!", ToastDuration.Short, 16);
    }

    public async Task<bool> SingIn(string email, string password)
    {
        if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                await Client.SignInWithEmailAndPasswordAsync(email, password);

                if(!Client.User.Info.IsEmailVerified)
                {
                    await Alerts[0].Show();
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                await Alerts[1].Show();
            }
        }
        else
        {
            await Alerts[2].Show();
        }

        return false;
    }

    public async Task SingUp(string email, string password)
    {
        if(Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                await Client.CreateUserWithEmailAndPasswordAsync(email, password);

                try
                {
                    var link = await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(email);
                    await Task.Run(() => SendEmail(email, link));

                    await Alerts[0].Show();
                }
                catch
                {
                    await Alerts[2].Show();
                }
            }
            catch
            {
                await Alerts[1].Show();
            }
        }
        else
        {
            await Alerts[2].Show();
        }
    }

    public void SendEmail(string email, string link)
    {
        EmailMessage.To.Add(new MailboxAddress("", email));
        EmailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = "<h2><a href=" + link + ">Сlick here</a> to verify your email address.</h2><br>"
        };

        using(var smtpClient = new SmtpClient())
        {
            smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
            smtpClient.Connect("smtp.yandex.ru", 465, true);
            smtpClient.Authenticate(PrivateConstants.Email, PrivateConstants.EmailPassword);
            smtpClient.Send(EmailMessage);
            smtpClient.Disconnect(true);
        }
    }
}
