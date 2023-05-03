using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.SimpleNote;

namespace MoodMate;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ChooseMoodPage), typeof(ChooseMoodPage));
        Routing.RegisterRoute(nameof(CreateOrEditMoodPage), typeof(CreateOrEditMoodPage));
        Routing.RegisterRoute(nameof(AnalysisMoodPage), typeof(AnalysisMoodPage));
        Routing.RegisterRoute(nameof(CreateOrEditNotePage), typeof(CreateOrEditNotePage));
        Routing.RegisterRoute(nameof(PlayMusicPage), typeof(PlayMusicPage));
    }
}
