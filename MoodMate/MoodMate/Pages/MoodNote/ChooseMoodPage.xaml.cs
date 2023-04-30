using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class ChooseMoodPage : ContentPage
{
	public ChooseMoodPage(ChooseMoodViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

    private async void Back_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }
}