using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class TimePickerPage : Popup
{
    public TimePickerPage(List<int>[] sbytes)
    {
        InitializeComponent();
        col1.ItemsSource = sbytes[0];
        col2.ItemsSource = sbytes[1];
        col3.ItemsSource = sbytes[1];
    }

    private void Ok_Clicked(object sender, EventArgs e)
    {
        Task.WaitAny();
        Close(col1.CurrentItem.ToString() + ':' + col2.CurrentItem.ToString() + ':' + col3.CurrentItem.ToString());
    }
    private void Cancel_Clicked(object sender, EventArgs e) => Close(null);
}