using CommunityToolkit.Maui.Views;
using MoodMate.Components.Entities;

namespace MoodMate.Pages.MoodNote;

public partial class ChangeMoodMenuPage : Popup
{
    FileService Moods;
    public ChangeMoodMenuPage(FileService fileServices)
    {
        InitializeComponent();
        Moods = fileServices;
        collection.ItemsSource = Moods.GetServiceData();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var item = ((Grid)sender).BindingContext as FileService;
        Close(new Tuple<string, string>(item.Source, item.Name));
    }
}