using MoodMate.ViewModels;

namespace MoodMate.Pages.SimpleNote;

public partial class CreateOrEditNotePage : ContentPage
{
    private readonly CreateOrEditNoteViewModel Model;
    public CreateOrEditNotePage(CreateOrEditNoteViewModel model)
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