using Google.Cloud.Firestore;
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
        ClearNotes();
        await NoteControl.UpdateFile(Constants.PathNotes);
    }
    public override async Task SaveLocalToCloud(Firebase.Auth.User user)
    {
        var data = GetData();
        while (data.Count != 0)
        {
            await Db.Collection("Users").Document(user.Uid).Collection("SimpleNote").
                AddAsync(new Dictionary<string, object>
                {
                    { "Date", TimeZoneInfo.ConvertTimeToUtc(data[0].Date) },
                    { "Text", data[0].Text }
                });
            data.RemoveAt(0);
        }
        await NoteControl.UpdateFile(Constants.PathNotes);
    }
    public override async Task LoadNoteCloud(int offset, int limit, Firebase.Auth.User user, bool refresh = true)
    {
        if (refresh)
            ClearNotes();

        var snapshot = await Db.Collection("Users").Document(user.Uid).
            Collection("SimpleNote").OrderByDescending("Date").Offset(offset).Limit(limit).GetSnapshotAsync();

        foreach (var item in snapshot)
        {
            var dictionary = item.ToDictionary();

            NoteControl.Data.Add(new SimpleNote
            {
                Id = item.Id,
                Date = ((Timestamp)dictionary["Date"]).ToDateTime().ToLocalTime(),
                Text = dictionary["Text"]?.ToString()
            });
        }
    }
    public override async Task AddNote(SimpleNote obj, Firebase.Auth.User user = null)
    {
        if (user != null)
        {
            var rezult = await Db.Collection("Users").Document(user.Uid).Collection("SimpleNote").
                AddAsync(new Dictionary<string, object>
                {
                    { "Date", TimeZoneInfo.ConvertTimeToUtc(obj.Date) },
                    { "Text", obj.Text }
                });
            obj.Id = rezult.Id;
            NoteControl.Add(obj);
        }
        else
        {
            obj.Id = NoteControl.GenerateKey();
            while (NoteControl.Data.Any(n => n.Id == obj.Id))
            {
                obj.Id = NoteControl.GenerateKey();
            }
            NoteControl.Add(obj);
            await NoteControl.UpdateFile(Constants.PathNotes);
        }
    }
    public override async Task ChangeNote(SimpleNote obj, Firebase.Auth.User user = null)
    {
        var index = NoteControl.Data.FindIndex(item => obj.Id == item.Id);
        if (index > -1)
        {
            if (user != null)
            {
                await Db.Collection("Users").Document(user.Uid).Collection("SimpleNote").
                    Document(obj.Id.ToString()).UpdateAsync(new Dictionary<string, object>
                    {
                        { "Date", TimeZoneInfo.ConvertTimeToUtc(obj.Date) },
                        { "Text", obj.Text }
                    });
                NoteControl.Change(index, obj);
            }
            else
            {
                NoteControl.Change(index, obj);
                await NoteControl.UpdateFile(Constants.PathNotes);
            }
        }
    }
    public override async Task DeleteNote(SimpleNote obj, Firebase.Auth.User user = null)
    {
        var index = NoteControl.Data.FindIndex(item => obj.Id == item.Id);
        if (index > -1)
        {
            if (user != null)
            {
                await Db.Collection("Users").Document(user.Uid).Collection("SimpleNote").
                    Document(obj.Id).DeleteAsync();
                NoteControl.Delete(index);
            }
            else
            {
                NoteControl.Delete(index);
                await NoteControl.UpdateFile(Constants.PathNotes);
            }
        }
    }
}
