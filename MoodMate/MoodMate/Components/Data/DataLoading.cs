using MoodMate.Components.Data.Abstractions;
using System.Text.Json;

namespace MoodMate.Components.Data;

public class DataLoading<T> : IDataLoading<T>
{
    public List<T> Data { get; set; }
    public async Task Load(string path, bool local)
    {
        try
        {
            string content;
            if (local)
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync(path);

                using (StreamReader reader = new StreamReader(stream))
                    content = await reader.ReadToEndAsync();
            }
            else
            {
                string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, path);

                using (StreamReader reader = new StreamReader(targetFile))
                    content = await reader.ReadToEndAsync();
            }
            Data = JsonSerializer.Deserialize<List<T>>(content);
        }
        catch
        {
            Data = new();
        }
    }
}
