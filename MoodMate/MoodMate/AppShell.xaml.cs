using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;

namespace MoodMate;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ChooseMoodPage), typeof(ChooseMoodPage));
        Routing.RegisterRoute(nameof(CreateOrEditMoodPage), typeof(CreateOrEditMoodPage));
    }
}
