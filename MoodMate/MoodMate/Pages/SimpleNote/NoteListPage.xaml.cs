using MoodMate.ViewModels;

namespace MoodMate.Pages.SimpleNote;

public partial class NoteListPage : ContentPage
{
    private readonly NoteListViewModel Model;
    public NoteListPage(NoteListViewModel model)
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