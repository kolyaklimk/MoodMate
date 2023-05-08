using MoodMate.ViewModels.Music;

namespace MoodMate.Pages.Music;

public partial class PlayMusicPage : ContentPage
{
    public PlayMusicPage(PlayMusicViewModel model)
    {
        InitializeComponent();
        BindingContext = model;

        MessagingCenter.Subscribe<PlayMusicViewModel>(this, "Start", (sender) =>
        {
            ImageSound.RelRotateTo(-30, 2000);
            ImageMusic.RelRotateTo(30, 2000);
        });
        MessagingCenter.Subscribe<PlayMusicViewModel>(this, "Stop", (sender) =>
        {
            ImageSound.CancelAnimations();
            ImageMusic.CancelAnimations();
        });
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}