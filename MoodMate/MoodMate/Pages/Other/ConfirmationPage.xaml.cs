using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class ConfirmationPage : Popup
{
    public ConfirmationPage(string text, string button)
    {
        InitializeComponent();
        Text.Text = text;
        TextButton.Text = button;
    }

    private void Cancel_Clicked(object sender, EventArgs e) => Close(null);
    private void Delete_Clicked(object sender, EventArgs e) => Close(true);
}