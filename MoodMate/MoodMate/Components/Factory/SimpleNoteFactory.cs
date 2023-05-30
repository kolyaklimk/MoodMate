using MoodMate.Components.Entities;
using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Factory;

internal class SimpleNoteFactory : INoteFactory<SimpleNote>
{
    public ANote<SimpleNote> CreateNote()
    {
        return new SimpleNote();
    }
}