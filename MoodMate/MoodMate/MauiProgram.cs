using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Factory;
using MoodMate.Messages;
using MoodMate.Pages.MoodNote;
using MoodMate.Pages.Music;
using MoodMate.Pages.Other;
using MoodMate.Pages.SimpleNote;
using MoodMate.ViewModels;
using MoodMate.ViewModels.Music;
using MoodMate.ViewModels.Other;
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
        // Pages and ViewModels
        services.AddSingleton<MusicListPage, MusicListViewModel>();
        services.AddTransient<PlayMusicPage, PlayMusicViewModel>();

        services.AddSingleton<NoteListPage, NoteListViewModel>();
        services.AddSingleton<CreateOrEditNotePage, CreateOrEditNoteViewModel>();

        services.AddSingleton<MoodListPage, MoodListViewModel>();
        services.AddSingleton<ChooseMoodPage, ChooseMoodViewModel>();
        services.AddSingleton<CreateOrEditMoodPage, CreateOrEditMoodViewModel>();
        services.AddSingleton<AnalysisMoodPage, AnalysisMoodViewModel>();

        services.AddSingleton<AuthenticationPage, AuthenticationViewModel>();

        // Services
        services.AddSingleton(_ => new Note[] { new Note("Mood"), new Note("Simple") });
        services.AddSingleton(_ => new FileService[] { new FileService(), new FileService(), new FileService() });
        services.AddSingleton(AudioManager.Current);
        services.AddSingleton(_ => new List<int>[] { new List<int>(), new List<int>() });
        services.AddSingleton(_ => new IToast[4]);
        services.AddSingleton<StopRotateMessage>();
        services.AddSingleton<StartRotateMessage>();
        services.AddSingleton<UpdateMoodNoteMessage>();
        services.AddSingleton<UpdateSimpleNoteMessage>();
        services.AddSingleton<IUser, User>();
    }
}
