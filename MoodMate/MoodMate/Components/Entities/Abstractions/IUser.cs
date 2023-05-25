using CommunityToolkit.Maui.Core;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using MailKit.Net.Smtp;
using MimeKit;

namespace MoodMate.Components.Entities.Abstractions;

public interface IUser
{
    FirebaseAuthClient Client { get; set; }
    MimeMessage EmailMessage { get; set; }
    IToast[] Alerts { get; set; }

    Task<bool> SingIn(string email, string password);
    Task SingUp(string email, string password);
    void SendEmail(string email, string link);
}
