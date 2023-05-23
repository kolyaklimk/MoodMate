using MoodMate.Pages.Music;

namespace MoodMate;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(PlayMusicPage), typeof(PlayMusicPage));
    }
}
