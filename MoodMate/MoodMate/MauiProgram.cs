using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.SimpleNote;
using MoodMate.ViewModels;

namespace MoodMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        SetupServices(builder.Services);

        return builder.Build();
    }

    private static void SetupServices(IServiceCollection services)
    {
        //Pages
        services.AddSingleton<MoodListPage>();
        services.AddSingleton<MusicListPage>();
        services.AddSingleton<NoteListPage>();
        //ViewModels
        services.AddSingleton<MoodListViewModel>();
    }
}
