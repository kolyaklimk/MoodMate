using MoodMate.ViewModels;

namespace MoodMate.Pages.SimpleNote;

public partial class CreateOrEditNotePage : ContentPage
{
    public CreateOrEditNotePage(CreateOrEditNoteViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}