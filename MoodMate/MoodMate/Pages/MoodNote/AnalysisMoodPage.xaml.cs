using MoodMate.ViewModels;

namespace MoodMate.Pages.MoodNote;

public partial class AnalysisMoodPage : ContentPage
{
    public AnalysisMoodPage(AnalysisMoodViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}