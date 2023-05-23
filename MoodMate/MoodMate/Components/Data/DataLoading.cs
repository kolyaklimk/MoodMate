using MoodMate.Components.Data.Abstractions;
using SerializationTools;

namespace MoodMate.Components.Data;

public class DataLoading<T>: IDataLoading<T>
{
    public List<T> Data { get; set; } = new();
    public async Task Load(string path, bool local)
    {
        try
        {
            if(local)
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync(path);
                Data = await DataSerializer.JsonDeserializeAsync<List<T>>(stream);
            }
            else
            {
                string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, path);
                using(StreamReader reader = new StreamReader(targetFile))
                    Data = await DataSerializer.JsonDeserializeAsync<List<T>>(reader.BaseStream);
            }
        }
        catch
        {
        }
    }
}
