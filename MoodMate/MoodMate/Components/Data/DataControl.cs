using MoodMate.Components.Data.Abstractions;
using MoodMate.Components.Data.Comparer;
using System.Text.Json;

namespace MoodMate.Components.Data;

public class DataControl<T> : DataLoading<T>, IDataControl<T>
{
    public void Add(T item)
    {
        Data.Add(item);
    }
    public void Change(int index, T item)
    {
        Data[index] = item;
    }
    public void Delete(int index)
    {
        Data.RemoveAt(index);
    }
    public async Task SortByDate(string sortColumn)
    {
        await Task.Run(() => Data.Sort(new GenericComparer<T>(sortColumn)));
    }
    public async Task UpdateFile(string path)
    {
        var json = JsonSerializer.Serialize(Data);

        try
        {
            string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, path);
            using (StreamWriter writer = new StreamWriter(targetFile, false))
            {
                await writer.WriteLineAsync(json);
            }
        }
        catch
        {
        }
    }
}