using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin.Auth;
using MailKit.Net.Smtp;
using MimeKit;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Pages.Other;
using MoodMate.Templates;
using SerializationTools;

namespace MoodMate.Components.Entities;

public class User : IUser
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

        Alerts = new IToast[6];
        Alerts[0] = Toast.Make("Check your email for verification!", ToastDuration.Short, 16);
        Alerts[1] = Toast.Make("Wrong email or password!", ToastDuration.Short, 16);
        Alerts[2] = Toast.Make("No internet connection!", ToastDuration.Short, 16);
        Alerts[3] = Toast.Make("Try again in 1 minute!", ToastDuration.Short, 16);
        Alerts[4] = Toast.Make("Check your email to reset your password!", ToastDuration.Short, 16);
    }

    public async Task<int> SingIn(string email, string password)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                await Client.SignInWithEmailAndPasswordAsync(email, password);

                if (!Client.User.Info.IsEmailVerified)
                {
                    try
                    {
                        await SendEmailVerificationLink();
                        await Alerts[0].Show();
                    }
                    catch
                    {
                        await Alerts[3].Show();
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                try
                {
                    await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

                    if (await Shell.Current.ShowPopupAsync(new ConfirmationPage("Reset your password?", "Reset")) != null)
                    {
                        await SendEmailPasswordResetLink(email);
                        return 2;
                    }
                    else
                    {
                        await Alerts[1].Show();
                    }
                }
                catch
                {
                    await Alerts[1].Show();
                }
            }
        }
        else
        {
            await Alerts[2].Show();
        }
        return 0;
    }

    public async Task SingUp(string email, string password)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                await Client.CreateUserWithEmailAndPasswordAsync(email, password);

                try
                {
                    await SendEmailVerificationLink();
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

    public void SignOut()
    {
        Client.SignOut();
    }

    public async Task DeleteUser()
    {
        await FirebaseAuth.DefaultInstance.DeleteUserAsync(Client.User.Uid);
    }

    public async Task SendEmailVerificationLink()
    {
        var link = await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(Client.User.Info.Email);
        await SendEmailLink(link, "verify your email address", Client.User.Info.Email);
    }

    public async Task SendEmailPasswordResetLink(string email)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            try
            {
                var link = await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
                await SendEmailLink(link, "reset the password", email);
                await Alerts[4].Show();
            }
            catch
            {
                await Alerts[3].Show();
            }
        }
        else
        {
            await Alerts[2].Show();
        }
    }

    public async Task SendEmailLink(string link, string text, string email)
    {
        await Task.Run(() =>
        {
            EmailMessage.To.Add(new MailboxAddress("", email));
            EmailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<h2><a href=" + link + ">Сlick here</a> to " + text + ".</h2><br>"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtpClient.Connect(PrivateConstants.host, PrivateConstants.port, PrivateConstants.useSsl);
                smtpClient.Authenticate(PrivateConstants.Email, PrivateConstants.EmailPassword);
                smtpClient.Send(EmailMessage);
                smtpClient.Disconnect(true);
            }
        });
    }

    public async Task SaveEmailAndPasswordLocal(string email, string password)
    {
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, Constants.PathUser);
        using (FileStream fs = new FileStream(targetFile, FileMode.Create))
        {
            await DataSerializer.JsonSerializeAsync(fs, new EmailAndPassword(email, password));
        }
    }

    public async Task<EmailAndPassword> LoadEmailAndPasswordLocal()
    {
        string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, Constants.PathUser);
        using (StreamReader reader = new StreamReader(targetFile))
            return await DataSerializer.JsonDeserializeAsync<EmailAndPassword>(reader.BaseStream);
    }
}
