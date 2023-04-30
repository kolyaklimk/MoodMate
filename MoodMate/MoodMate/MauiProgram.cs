using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoodMate.Components;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
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
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        SetupServices(builder.Services);

        return builder.Build();
    }

    private static void SetupServices(IServiceCollection services)
    {
        //// ViewModels
        services.AddSingleton<MoodListViewModel>();
        services.AddSingleton<ChooseMoodViewModel>();
        services.AddTransient<CreateOrEditMoodViewModel>();

        //// Pages
        services.AddSingleton<MusicListPage>();

        services.AddSingleton<NoteListPage>();

        services.AddSingleton<MoodListPage>();
        services.AddSingleton<ChooseMoodPage>();
        services.AddTransient<CreateOrEditMoodPage>();

        //// Service
        services.AddSingleton(_ => new Note[] { new Note("Mood"), new Note("Simple") });
        services.AddSingleton(new FileService());
    }
}
