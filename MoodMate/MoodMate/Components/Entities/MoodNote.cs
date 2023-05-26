using MoodMate.Components.Data;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Templates;

namespace MoodMate.Components.Entities;

public class MoodNote : ANote<MoodNote>, IMoodNoteAnalysis
{
    public DataAnalysis<MoodNote> MoodAnalysis { get; set; } = new();
    public FileService Mood { get; set; } = new();
    public MoodNote() { }
    public MoodNote(DateTime date, string name, string sourse, string text)
    {
        Date = date;
        Mood.Name = name;
        Mood.Source = sourse;
        Text = text;
    }
    public override async Task LoadNote()
    {
        await NoteControl.Load(Constants.PathMoodNotes, false);
    }
    public override async Task AddNote(MoodNote obj)
    {
        if (NoteControl.Data.Count > 0)
            obj.Id = NoteControl.Data.Last().Id + 1;
        NoteControl.Add(obj);
        await NoteControl.UpdateFile(Constants.PathMoodNotes);
    }
    public override async Task ChangeNote(MoodNote obj, uint id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Change(index, obj);
            await NoteControl.UpdateFile(Constants.PathMoodNotes);
        }
    }
    public override async Task DeleteNote(uint id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Delete(index);
            await NoteControl.UpdateFile(Constants.PathMoodNotes);
        }
    }

    // Analyse methot's
    public async Task InitAnalyse(DateTime date)
    {
        MoodAnalysis.AnalysedData.Clear();
        await Task.Run(() =>
        {
            foreach (var item in NoteControl.Data)
            {
                MoodAnalysis.AddItem(item.Mood.Name, item.Mood.Source, item.Date, date);
            }
        });
    }
    public List<MyKeyValue> GetAnalysedData()
    {
        return MoodAnalysis.AnalysedData.Select(kvp => new MyKeyValue
        {
            Key = kvp.Key,
            Value1 = kvp.Value.Item1,
            Value2 = kvp.Value.Item2,
            Value3 = kvp.Value.Item3,
        }).ToList();
    }
    public int GetCountMood()
    {
        return MoodAnalysis.GetCount();
    }
    public void FindPercentsMood()
    {
        MoodAnalysis.GetPercents();
    }
}