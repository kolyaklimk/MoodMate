using CommunityToolkit.Maui.Views;
using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class ChangeMoodMenuPage : Popup
{
	public ChangeMoodMenuPage(ChooseMoodViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

}