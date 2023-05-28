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
    public override async Task DeleteNoteLocal()
    {
        await NoteControl.Load(Constants.PathNotes, false);
    }
    public override async Task AddNote(SimpleNote obj, Firebase.Auth.User user = null)
    {
        if (NoteControl.Data.Count > 0)
            obj.Id = NoteControl.Data.Last().Id + 1;
        NoteControl.Add(obj);
        await NoteControl.UpdateFile(Constants.PathNotes);
    }
    public override async Task ChangeNote(SimpleNote obj, Firebase.Auth.User user = null)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == obj.Id);
        if (index > -1)
        {
            NoteControl.Change(index, obj);
            await NoteControl.UpdateFile(Constants.PathNotes);
        }
    }
    public override async Task DeleteNote(SimpleNote obj, Firebase.Auth.User user = null)
    {
        var index = NoteControl.Data.FindIndex(item => item.Id == obj.Id);
        if (index > -1)
        {
            NoteControl.Delete(index);
            await NoteControl.UpdateFile(Constants.PathNotes);
        }
    }

    public override Task SaveLocalToCloud(Firebase.Auth.User user)
    {
        throw new NotImplementedException();
    }

    public override Task LoadNoteCloud(int offset, int limit, Firebase.Auth.User user, bool refresh = true)
    {
        throw new NotImplementedException();
    }
}
