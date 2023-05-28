using MoodMate.Components.Data.Abstractions;
using SerializationTools;

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
    public void ClearData()
    {
        Data.Clear();
    }
    public async Task UpdateFile(string path)
    {
        try
        {
            string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, path);
            using (FileStream fs = new FileStream(targetFile, FileMode.Create))
            {
                await DataSerializer.JsonSerializeAsync(fs, Data);
            }
        }
        catch { }
    }
    public string GenerateKey()
    {
        var random = new Random();
        return new string(Enumerable.Range(1, 16).
            Select(x => Constants.ForGenerateKey[random.Next(Constants.ForGenerateKey.Length)]).ToArray());
    }
}