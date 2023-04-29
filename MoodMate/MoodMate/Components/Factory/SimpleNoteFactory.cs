using MoodMate.Components.Entities.Abstractions;
using MoodMate.Components.Entities;

namespace MoodMate.Components.Factory;

internal class SimpleNoteFactory : INoteFactory<SimpleNote>
{
    public ANote<SimpleNote> CreateNote()
    {
        return new SimpleNote();
    }
}