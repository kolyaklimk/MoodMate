using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class MoodListPage : ContentPage
{
	public MoodListPage(MoodListViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}