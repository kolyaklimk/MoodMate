using CommunityToolkit.Mvvm.ComponentModel;
using MoodMate.Components.Data;

namespace MoodMate.Components.Entities.Abstractions;

public abstract partial class ANote<T> : ObservableObject
{
    protected DataControl<T> NoteControl { get; set; } = new();
    [ObservableProperty] public int id;
    [ObservableProperty] public DateTime date;
    [ObservableProperty] public string text;
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
