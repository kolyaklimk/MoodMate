using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoodMate.Messages;

public class LoadedSimpleNoteMessage : ValueChangedMessage<object>
{
    public LoadedSimpleNoteMessage() : base(null) { }
}
