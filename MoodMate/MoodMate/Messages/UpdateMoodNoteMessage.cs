using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoodMate.Messages;

public class UpdateMoodNoteMessage : ValueChangedMessage<object>
{
    public UpdateMoodNoteMessage() : base(null) { }
}
