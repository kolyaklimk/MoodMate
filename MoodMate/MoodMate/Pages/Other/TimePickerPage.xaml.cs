using CommunityToolkit.Maui.Views;

namespace MoodMate.Pages.Other;

public partial class TimePickerPage : Popup
{
    public List<int> Seconds = Enumerable.Range(0, 60).ToList();
    public List<int> Minutes = Enumerable.Range(0, 60).ToList();
    public List<int> Hours = Enumerable.Range(0, 24).ToList();
    public TimePickerPage()
    {
        InitializeComponent();
        col1.ItemsSource = Hours;
        col2.ItemsSource = Minutes;
        col3.ItemsSource = Seconds;
    }

    private void Ok_Clicked(object sender, EventArgs e) =>
        Close(col1.SelectedItem.ToString() + ':' + col2.SelectedItem.ToString() + ':' + col3.SelectedItem.ToString());
    private void Cancel_Clicked(object sender, EventArgs e) => Close(null);
}