using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class ChooseMoodPage : ContentPage
{
    private readonly ChooseMoodViewModel Model;
    public ChooseMoodPage(ChooseMoodViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        Model = model;
    }

    protected override bool OnBackButtonPressed()
    {
        Model.Back_Clicked();
        return true;
    }
}