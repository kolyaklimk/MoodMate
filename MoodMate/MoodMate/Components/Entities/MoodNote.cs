using MoodMate.Components.Data;
using MoodMate.Components.Entities.Abstractions;

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
        NoteControl.Add(obj);
        await NoteControl.SortByDate(Constants.SortByData);
        UpdateAllId();
        await NoteControl.UpdateFile(Constants.PathMoodNotes);
    }
    public override async Task ChangeNote(MoodNote obj, int id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Change(index, obj);
            await NoteControl.SortByDate(Constants.SortByData);
            UpdateAllId();
            await NoteControl.UpdateFile(Constants.PathMoodNotes);
        }
    }
    public override async Task DeleteNote(int id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Delete(index);
            await NoteControl.SortByDate(Constants.SortByData);
            UpdateAllId();
            await NoteControl.UpdateFile(Constants.PathMoodNotes);
        }
    }
    public override void UpdateAllId()
    {
        foreach (var item in NoteControl.Data.Select((value, index) => new { value, index }))
        {
            item.value.Id = item.index;
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
    public List<KeyValuePair<string, (string, int, int)>> GetAnalysedData()
    {
        return MoodAnalysis.AnalysedData.ToList();
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