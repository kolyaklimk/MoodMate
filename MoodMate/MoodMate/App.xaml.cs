using MoodMate.Components;
using MoodMate.Components.Entities;
using MoodMate.Components.Factory;

namespace MoodMate;

public partial class App : Application
{
    private Note[] notes;
    private FileService[] fileServices;

    public App(Note[] notes, FileService[] fileServices)
    {
        InitializeComponent();
        this.notes = notes;
        this.fileServices = fileServices;
        LoadeService();
        MainPage = new AppShell();
    }
    private async void LoadeService()
    {
        await notes[0].note.LoadNote();
        await notes[1].note.LoadNote();
        await fileServices[0].LoadService(Constants.PathMoods);
        await fileServices[1].LoadService(Constants.PathMusic);
        await fileServices[2].LoadService(Constants.PathSound);
    }
}
