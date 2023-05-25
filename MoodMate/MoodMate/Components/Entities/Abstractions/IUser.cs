using CommunityToolkit.Maui.Core;
using Firebase.Auth;
using MimeKit;
using MoodMate.Templates;

namespace MoodMate.Components.Entities.Abstractions;

public interface IUser
{
    FirebaseAuthClient Client { get; set; }
    MimeMessage EmailMessage { get; set; }
    IToast[] Alerts { get; set; }

    Task<bool> SingIn(string email, string password);
    Task SingUp(string email, string password);
    Task SendEmailVerificationLink(string email);
    Task SaveEmailAndPasswordLocal(string email, string password);
    Task<EmailAndPassword> LoadEmailAndPasswordLocal();
}
