using MoodMate.ViewModels.Other;

namespace MoodMate.Pages.Other;

public partial class UserPage : ContentPage
{
    private readonly UserViewModel Model;
    public UserPage(UserViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
        Model = model;
    }

    protected override bool OnBackButtonPressed()
    {
        Model.GoToMoodPage();
        return true;
    }
}