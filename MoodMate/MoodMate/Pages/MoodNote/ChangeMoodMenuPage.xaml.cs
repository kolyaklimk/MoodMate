using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.MoodNote;

public partial class ChangeMoodMenuPage : Popup
{
	public ChangeMoodMenuPage(ChooseMoodPage model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}