using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class ContextMenuPage : Popup
{
    public ContextMenuPage()
    {
        InitializeComponent();
    }

    private void Button_Clicked_Share(object sender, EventArgs e) => Close(1);
    private void Button_Clicked_Delete(object sender, EventArgs e) => Close(2);
    private void Button_Clicked_Edit(object sender, EventArgs e) => Close(3);
    private void Button_Clicked_Cancel(object sender, EventArgs e) => Close(4);
}