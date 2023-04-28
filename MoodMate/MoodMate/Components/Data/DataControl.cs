using MoodMate.Components.Data.Abstractions;
using System.Diagnostics;

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
    }
    public async Task UpdateFile(string path)
    {
    }
}