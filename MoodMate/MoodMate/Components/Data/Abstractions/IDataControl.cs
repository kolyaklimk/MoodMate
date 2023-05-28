namespace MoodMate.Components.Data.Abstractions;

internal interface IDataControl<T>
{
    void Delete(int index);
    void Add(T item);
    void Change(int index, T item);
    Task UpdateFile(string path);
    string GenerateKey();
}
