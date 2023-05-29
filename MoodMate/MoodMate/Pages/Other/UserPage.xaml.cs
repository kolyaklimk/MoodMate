using MoodMate.ViewModels.Other;

namespace MoodMate.Pages.Other;

public partial class UserPage : ContentPage
{
	public UserPage(UserViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}