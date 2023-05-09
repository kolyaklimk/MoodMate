using CommunityToolkit.Mvvm.Messaging;
using MoodMate.Messages;
using MoodMate.ViewModels.Music;

namespace MoodMate.Pages.Music;

public partial class PlayMusicPage : ContentPage, IRecipient<StopRotateMessage>, IRecipient<StartRotateMessage>
{
    public PlayMusicPage(PlayMusicViewModel model)
    {
        InitializeComponent();
        BindingContext = model;
        WeakReferenceMessenger.Default.Register<StopRotateMessage>(this);
        WeakReferenceMessenger.Default.Register<StartRotateMessage>(this);
    }

    public void Receive(StopRotateMessage message)
    {
        ImageSound.CancelAnimations();
        ImageMusic.CancelAnimations();
    }

    public void Receive(StartRotateMessage message)
    {
        ImageSound.RelRotateTo(-7, 1200);
        ImageMusic.RelRotateTo(10, 1200);
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}