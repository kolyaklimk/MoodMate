using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoodMate.Messages;

public class StartRotateMessage : ValueChangedMessage<object>
{
    public StartRotateMessage() : base(null) { }
}
