using MoodMate.Components.Data;

namespace MoodMate.Components.Entities.Abstractions;

public abstract class ANote<T>
{
    protected DataControl<T> NoteControl { get; set; } = new();
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Text { get; set; }
    public List<T> GetData()
    {
        return NoteControl.Data.ToList();
    }
    public abstract Task LoadNote();
    public abstract Task AddNote(T obj);
    public abstract Task ChangeNote(T obj, int id);
    public abstract Task DeleteNote(int id);
    public abstract void UpdateAllId();
}
