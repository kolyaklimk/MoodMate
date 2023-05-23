using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class CreateOrEditMoodPage : ContentPage
{
    private readonly CreateOrEditMoodViewModel Model;
    public CreateOrEditMoodPage(CreateOrEditMoodViewModel model)
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