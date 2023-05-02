using MoodMate.ViewModels;

namespace MoodMate.Pages.SimpleNote;

public partial class NoteListPage : ContentPage
{
	public NoteListPage(NoteListViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}