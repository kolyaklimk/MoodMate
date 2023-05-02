using MoodMate.ViewModels.Music;

namespace MoodMate.Pages.Music;

public partial class MusicListPage : ContentPage
{
    public MusicListPage(MusicListViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}