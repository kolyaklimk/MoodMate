using MoodMate.Components.Data;

namespace MoodMate.Components.Entities.Abstractions;

internal interface INoteAnalysis
{
    DataAnalysis<MoodNote> MoodAnalysis { get; set; }
    void InitAnalyse(DateTime date);
    List<KeyValuePair<string, (string, int, int)>> GetAnalysedData();
    int GetCountMood();
    void FindPercentsMood();
}
