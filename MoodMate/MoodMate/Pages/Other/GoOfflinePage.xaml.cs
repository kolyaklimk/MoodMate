using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class GoOfflinePage : Popup
{
    public GoOfflinePage(string text = "")
    {
        InitializeComponent();
        Text.Text += text;
    }

    private void Offline_Clicked(object sender, EventArgs e) => Close(true);
    private void Cancel_Clicked(object sender, EventArgs e) => Close(false);
}