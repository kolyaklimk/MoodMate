using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoodMate.Messages;

public class UpdateSimpleNoteMessage : ValueChangedMessage<object>
{
    public UpdateSimpleNoteMessage() : base(null) { }
}
