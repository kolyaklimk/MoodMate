namespace MoodMate.Components.Data.Abstractions;

internal interface IDataAnalysis<T>
{
    public Dictionary<string, (string, int, int)> AnalysedData { get; set; }
    void AddItem(string name, string source, DateTime date, DateTime Choosedate);
    int GetCount();
    void GetPercents();
}
