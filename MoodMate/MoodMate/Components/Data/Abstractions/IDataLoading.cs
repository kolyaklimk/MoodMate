namespace MoodMate.Components.Data.Abstractions;

internal interface IDataLoading<T>
{
    List<T> Data { get; set; }
    Task Load(string path, bool local);
}
