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
    public void CreateDb(string projectId)
    {
        Db = FirestoreDb.Create(projectId);
    }
    public abstract List<T> GetDataSortByDate();
    public abstract Task LoadNoteLocal();
    public abstract Task LoadNoteCloudAndSaveLocal(Firebase.Auth.User user);
    public abstract Task LoadNoteCloud(Firebase.Auth.User user);
    public abstract Task AddNote(T obj, Firebase.Auth.User user = null);
    public abstract Task ChangeNote(T obj, Firebase.Auth.User user = null);
    public abstract Task DeleteNote(T obj, Firebase.Auth.User user = null);
}
