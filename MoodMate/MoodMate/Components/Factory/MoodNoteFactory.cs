using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Entities;

namespace MoodMate.Components.Factory;

internal class MoodNoteFactory : INoteFactory<MoodNote>
{
    public ANote<MoodNote> CreateNote()
    {
        return new MoodNote();
    }
}
