using MoodMate.ViewModels.Music;

namespace MoodMate.Pages.Music;

public partial class PlayMusicPage : ContentPage
{
	public PlayMusicPage(PlayMusicViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}