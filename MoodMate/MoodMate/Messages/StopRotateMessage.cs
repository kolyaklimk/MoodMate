using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoodMate.Messages;

public class StopRotateMessage : ValueChangedMessage<object>
{
    public StopRotateMessage() : base(null) { }
}
