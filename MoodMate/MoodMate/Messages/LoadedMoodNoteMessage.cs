using CommunityToolkit.Mvvm.Messaging.Messages;


namespace MoodMate.Messages;

internal class LoadedMoodNoteMessage : ValueChangedMessage<object>
{
    public LoadedMoodNoteMessage() : base(null) { }
}
