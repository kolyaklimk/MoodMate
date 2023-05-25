namespace MoodMate.Templates;

public class EmailAndPassword
{
    public string Email { get; set; }
    public string Password { get; set; }

    public EmailAndPassword(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
