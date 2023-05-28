using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class CloudWarningPage : Popup
{
	public CloudWarningPage()
	{
		InitializeComponent();
	}

    private void Offline_Clicked(object sender, EventArgs e) => Close(1);
    private void Delete_Clicked(object sender, EventArgs e) => Close(2);
    private void Synchronize_Clicked(object sender, EventArgs e) => Close(3);
}