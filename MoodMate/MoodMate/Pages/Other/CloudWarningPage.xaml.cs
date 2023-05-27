using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class CloudWarningPage : Popup
{
	public CloudWarningPage()
	{
		InitializeComponent();
	}

    private void Cancel_Clicked(object sender, EventArgs e) => Close(false);
    private void Synchronize_Clicked(object sender, EventArgs e) => Close(true);
}