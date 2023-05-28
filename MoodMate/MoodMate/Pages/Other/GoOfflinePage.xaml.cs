using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class GoOfflinePage : Popup
{
	public GoOfflinePage()
	{
		InitializeComponent();
	}

    private void Offline_Clicked(object sender, EventArgs e) => Close(true);
    private void Cancel_Clicked(object sender, EventArgs e) => Close(false);
}