using MoodMate.Components.Data.Abstractions;

namespace MoodMate.Components.Data;

public class DataAnalysis<T> : IDataAnalysis<T>
{
    // Dictionary<name, (source, count of element, procent)> 
    public Dictionary<string, (string, int, int)> AnalysedData { get; set; } = new();

    public void AddItem(string name, string source, DateTime date, DateTime Choosedate)
    {
        if (date.Month == Choosedate.Month && date.Year == Choosedate.Year)
        {
            if (AnalysedData.ContainsKey(name))
            {
                AnalysedData[name] = (AnalysedData[name].Item1, AnalysedData[name].Item2 + 1, AnalysedData[name].Item3);
            }
            else
            {
                AnalysedData.Add(name, (source, 1, -1));
            }
        }
    }

    public int GetCount()
    {
        int count = 0;
        foreach (var item in AnalysedData)
        {
            count += item.Value.Item2;
        }
        return count;
    }

    public void GetPercents()
    {
        int count = GetCount();
        foreach (var item in AnalysedData)
        {
            AnalysedData[item.Key] = (item.Value.Item1, item.Value.Item2, item.Value.Item2 * 100 / count);
        }
    }
}
