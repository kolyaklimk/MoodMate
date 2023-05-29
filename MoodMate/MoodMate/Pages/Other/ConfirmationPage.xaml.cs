using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class ConfirmationPage : Popup
{
    public ConfirmationPage(string text)
    {
        InitializeComponent();
        Text.Text = text;
    }

    private void Cancel_Clicked(object sender, EventArgs e) => Close(null);
    private void Delete_Clicked(object sender, EventArgs e) => Close(true);
}