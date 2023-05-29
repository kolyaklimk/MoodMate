using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class ConfirmationPage : Popup
{
    public ConfirmationPage()
    {
        InitializeComponent();
    }

    private void Cancel_Clicked(object sender, EventArgs e) => Close(null);
    private void Delete_Clicked(object sender, EventArgs e) => Close(true);
}