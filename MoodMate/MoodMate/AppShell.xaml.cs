using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.SimpleNote;

namespace MoodMate;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PlayMusicPage), typeof(PlayMusicPage));
    }
}
