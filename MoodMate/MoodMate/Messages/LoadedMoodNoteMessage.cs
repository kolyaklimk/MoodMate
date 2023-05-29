using CommunityToolkit.Mvvm.Messaging.Messages;


namespace MoodMate.Messages;

public class LoadedMoodNoteMessage : ValueChangedMessage<object>
{
    public LoadedMoodNoteMessage() : base(null) { }
}
