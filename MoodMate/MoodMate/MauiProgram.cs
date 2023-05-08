using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.SimpleNote;
using MoodMate.ViewModels;
using MoodMate.ViewModels.Music;
using Plugin.Maui.Audio;

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

        SetupServices(builder.Services);

        return builder.Build();
    }

    private static void SetupServices(IServiceCollection services)
    {
        //// ViewModels
        services.AddSingleton<MusicListViewModel>();
        services.AddTransient<PlayMusicViewModel>();

        services.AddSingleton<NoteListViewModel>();
        services.AddSingleton<CreateOrEditNoteViewModel>();

        services.AddSingleton<MoodListViewModel>();
        services.AddSingleton<ChooseMoodViewModel>();
        services.AddSingleton<CreateOrEditMoodViewModel>();
        services.AddSingleton<AnalysisMoodViewModel>();

        //// Pages
        services.AddSingleton<MusicListPage>();
        services.AddTransient<PlayMusicPage>();

        services.AddSingleton<NoteListPage>();
        services.AddSingleton<CreateOrEditNotePage>();

        services.AddSingleton<MoodListPage>();
        services.AddSingleton<ChooseMoodPage>();
        services.AddSingleton<CreateOrEditMoodPage>();
        services.AddSingleton<AnalysisMoodPage>();

        //// Service
        services.AddSingleton(_ => new Note[] { new Note("Mood"), new Note("Simple") });
        services.AddSingleton(_ => new FileService[] { new FileService(), new FileService(), new FileService() });
        services.AddSingleton(AudioManager.Current);
        services.AddSingleton(_ => new List<int>[] { new List<int>(), new List<int>() });
    }
}
