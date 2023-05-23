using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class AnalysisMoodPage : ContentPage
{
    private readonly AnalysisMoodViewModel Model;
    public AnalysisMoodPage(AnalysisMoodViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        Model = model;
    }

    protected override bool OnBackButtonPressed()
    {
        Model.BackClick();
        return true;
    }
}