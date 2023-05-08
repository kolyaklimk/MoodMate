using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class ChooseMoodPage : ContentPage
{
    public ChooseMoodPage(ChooseMoodViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}