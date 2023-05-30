using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoodMate.Messages;

internal class LoadedSimpleNoteMessage : ValueChangedMessage<object>
{
    public LoadedSimpleNoteMessage() : base(null) { }
}
