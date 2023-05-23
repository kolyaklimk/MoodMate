using MoodMate.ViewModels.Music;

namespace MoodMate.Pages.Music;

public partial class MusicListPage : ContentPage
{
    private readonly MusicListViewModel Model;
    public MusicListPage(MusicListViewModel model)
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