using MoodMate.Components.Entities.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace MoodMate.Components.Entities;

internal class MoodNote : ANote<MoodNote>
{
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
}