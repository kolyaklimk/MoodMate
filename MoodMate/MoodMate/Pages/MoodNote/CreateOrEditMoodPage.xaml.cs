using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class CreateOrEditMoodPage : ContentPage
{
	public CreateOrEditMoodPage(CreateOrEditMoodViewModel model)
	{
		InitializeComponent();
		BindingContext= model;
	}
}