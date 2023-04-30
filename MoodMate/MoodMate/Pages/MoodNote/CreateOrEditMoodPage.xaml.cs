using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class CreateOrEditMoodPage : ContentPage
{
	public CreateOrEditMoodPage(CreateOrEditMoodViewModel model)
	{
		InitializeComponent();
		BindingContext= model;
	}

    private async void Back_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopAsync();
    }

    private async void Ok_Clicked(object sender, EventArgs e)
    {
        if (NameMood.Text != "")
            await Navigation.PopToRootAsync();
    }
}