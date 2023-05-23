using MoodMate.Components.Entities.Abstractions;

namespace MoodMate.Components.Entities;

public class SimpleNote : ANote<SimpleNote>
{
    public SimpleNote() { }
    public SimpleNote(DateTime date, string text)
    {
        Date = date;
        Text = text;
    }
    public override async Task LoadNote()
    {
        await NoteControl.Load(Constants.PathNotes, false);
    }
    public override async Task AddNote(SimpleNote obj)
    {
        NoteControl.Add(obj);
        await NoteControl.SortByDate(Constants.SortByData);
        UpdateAllId();
        await NoteControl.UpdateFile(Constants.PathNotes);
    }
    public override async Task ChangeNote(SimpleNote obj, int id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Change(index, obj);
            await NoteControl.SortByDate(Constants.SortByData);
            UpdateAllId();
            await NoteControl.UpdateFile(Constants.PathNotes);
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
            await NoteControl.UpdateFile(Constants.PathNotes);
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
