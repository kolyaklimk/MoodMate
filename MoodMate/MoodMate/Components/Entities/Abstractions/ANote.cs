using CommunityToolkit.Mvvm.ComponentModel;
using Google.Cloud.Firestore;
using MoodMate.Components.Data;

namespace MoodMate.Components.Entities.Abstractions;

public abstract partial class ANote<T> : ObservableObject
{
    protected DataControl<T> NoteControl { get; set; } = new();
    protected FirestoreDb Db { get; set; }
    [ObservableProperty] public uint id;
    [ObservableProperty] public DateTime date;
    [ObservableProperty] public string text;
    public List<T> GetData()
    {
        return NoteControl.Data.ToList();
    }
    public abstract List<T> GetDataSortByDate();
    public abstract Task LoadNoteLocal();
    public abstract Task LoadNoteCloudAndSaveLocal(string uid);
    public abstract Task LoadNoteCloud(string uid);
    public abstract Task AddNote(T obj, string uid = null);
    public abstract Task ChangeNote(T obj, string uid = null);
    public abstract Task DeleteNote(T obj, string uid = null);
}
