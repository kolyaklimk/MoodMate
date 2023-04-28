using MoodMate.Components.Data.Abstractions;

namespace MoodMate.Components.Data;

public class DataAnalysis<T> : IDataAnalysis<T>
{
    // Dictionary<name, (source, count of element, procent)> 
    public Dictionary<string, (string, int, int)> AnalysedData { get; set; } = new();
    public void AddItem(string name, string source, DateTime date, DateTime Choosedate)
    {
    }
    public int GetCount()
    {
    }
    public void GetPercents()
    {
    }
}
