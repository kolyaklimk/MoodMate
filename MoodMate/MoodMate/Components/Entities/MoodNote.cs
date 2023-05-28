using Google.Cloud.Firestore;
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
    public override List<MoodNote> GetDataSortByDate()
    {
        return NoteControl.Data.OrderByDescending(x => x.Date).ToList();
    }
    public override async Task LoadNoteLocal()
    {
        await NoteControl.Load(Constants.PathMoodNotes, false);
    }
    public override async Task LoadNoteCloudAndSaveLocal(Firebase.Auth.User user)
    {
        var snapshot = await Db.Collection("Users").Document(user.Uid).Collection("MoodNote").GetSnapshotAsync();
        List<MoodNote> cloudData = new();

        foreach (var item in snapshot)
        {
            var dictionary = item.ToDictionary();

            cloudData.Add(new MoodNote
            {
                Id = uint.Parse(dictionary["Id"].ToString()),
                Date = ((Timestamp)dictionary["Date"]).ToDateTime(),
                Text = dictionary["Text"]?.ToString(),
                Mood = new()
                {
                    Name = dictionary["Name"]?.ToString(),
                    Source = dictionary["Source"]?.ToString()
                }
            });
        }

        uint lastId = 0;
        if (cloudData.Count() != 0)
            lastId = cloudData.Last().Id;

        foreach (var item in NoteControl.Data.Select((value, index) => new { value, index }))
        {
            item.value.Id = (uint)item.index + lastId + 1;
            await Db.Collection("Users").Document(user.Uid).Collection("MoodNote").
                Document(item.value.Id.ToString()).SetAsync(new Dictionary<string, object>
                {
                    { "Id", item.value.Id },
                    { "Date", item.value.Date },
                    { "Text", item.value.Text },
                    { "Name", item.value.Mood.Name },
                    { "Source", item.value.Mood.Source }
                });
        }

        cloudData.AddRange(NoteControl.Data);

        NoteControl.Data.Clear();
        await NoteControl.UpdateFile(Constants.PathMoodNotes);

        NoteControl.Data = cloudData;
    }
    public override async Task LoadNoteCloud(Firebase.Auth.User user)
    {
        NoteControl.Data.Clear();
        var snapshot = await Db.Collection("Users").Document(user.Uid).
            Collection("MoodNote").GetSnapshotAsync();

        foreach (var item in snapshot)
        {
            var dictionary = item.ToDictionary();

            NoteControl.Data.Add(new MoodNote
            {
                Id = uint.Parse(dictionary["Id"].ToString()),
                Date = ((Timestamp)dictionary["Date"]).ToDateTime(),
                Text = dictionary["Text"]?.ToString(),
                Mood = new()
                {
                    Name = dictionary["Name"]?.ToString(),
                    Source = dictionary["Source"]?.ToString()
                }
            });
        }
    }
    public override async Task AddNote(MoodNote obj, Firebase.Auth.User user = null)
    {
        if (NoteControl.Data.Count > 0)
            obj.Id = NoteControl.Data.Last().Id + 1;

        if (user != null)
        {
            await Db.Collection("Users").Document(user.Uid).Collection("MoodNote").
                Document(obj.Id.ToString()).SetAsync(new Dictionary<string, object>
                {
                    { "Id", obj.Id },
                    { "Date", obj.Date },
                    { "Text", obj.Text },
                    { "Name", obj.Mood.Name },
                    { "Source", obj.Mood.Source }
                });
            NoteControl.Add(obj);
        }
        else
        {
            NoteControl.Add(obj);
            await NoteControl.UpdateFile(Constants.PathMoodNotes);
        }
    }
    public override async Task ChangeNote(MoodNote obj, Firebase.Auth.User user = null)
    {
        var index = NoteControl.Data.FindIndex(item => obj.Id == item.Id);
        if (index > -1)
        {
            if (user != null)
            {
                await Db.Collection("Users").Document(user.Uid).Collection("MoodNote").
                    Document(obj.Id.ToString()).UpdateAsync(new Dictionary<string, object>
                    {
                        { "Date", obj.Date },
                        { "Text", obj.Text },
                        { "Name", obj.Mood.Name },
                        { "Source", obj.Mood.Source }
                    });
                NoteControl.Change(index, obj);
            }
            else
            {
                NoteControl.Change(index, obj);
                await NoteControl.UpdateFile(Constants.PathMoodNotes);
            }
        }
    }
    public override async Task DeleteNote(MoodNote obj, Firebase.Auth.User user = null)
    {
        var index = NoteControl.Data.FindIndex(item => obj.Id == item.Id);
        if (index > -1)
        {
            if (user != null)
            {
                await Db.Collection("Users").Document(user.Uid).Collection("MoodNote").
                    Document(obj.Id.ToString()).DeleteAsync();
                NoteControl.Delete(index);
            }
            else
            {
                NoteControl.Delete(index);
                await NoteControl.UpdateFile(Constants.PathMoodNotes);
            }
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