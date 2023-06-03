using MoodMate.Components.Data;
using MoodMate.Templates;

namespace MoodMate.Components.Entities.Abstractions;

internal interface IMoodNoteAnalysis
{
    DataAnalysis<MoodNote> MoodAnalysis { get; set; }
    Task InitAnalyse(DateTime date, Firebase.Auth.User user = null);
    List<MyKeyValue> GetAnalysedData();
    int GetCountMood();
    void FindPercentsMood();
}
