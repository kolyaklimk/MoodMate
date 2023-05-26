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
    public override List<SimpleNote> GetDataSortByDate()
    {
        return NoteControl.Data.OrderByDescending(x => x.Date).ToList();
    }
    public override async Task LoadNoteLocal()
    {
        await NoteControl.Load(Constants.PathNotes, false);
    }
    public override async Task AddNote(SimpleNote obj)
    {
        if (NoteControl.Data.Count > 0)
            obj.Id = NoteControl.Data.Last().Id + 1;
        NoteControl.Add(obj);
        await NoteControl.UpdateFile(Constants.PathNotes);
    }
    public override async Task ChangeNote(SimpleNote obj, uint id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Change(index, obj);
            await NoteControl.UpdateFile(Constants.PathNotes);
        }
    }
    public override async Task DeleteNote(uint id)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == id);
        if (index > -1)
        {
            NoteControl.Delete(index);
            await NoteControl.UpdateFile(Constants.PathNotes);
        }
    }
}
