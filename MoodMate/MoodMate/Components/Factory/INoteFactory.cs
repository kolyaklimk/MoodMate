using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Factory;

internal interface INoteFactory<T>
{
    ANote<T> CreateNote();
}
