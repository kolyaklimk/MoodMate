using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Factory;

internal class MoodNoteFactory : INoteFactory<MoodNote>
{
    public ANote<MoodNote> CreateNote()
    {
        return new MoodNote();
    }
}
