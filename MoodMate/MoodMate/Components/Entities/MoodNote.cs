using FirebaseAdmin;
using Google.Cloud.Firestore;
using MoodMate.Components.Data;
using MoodMate.Components.Entities.Abstractions;
using MoodMate.Templates;

namespace MoodMate.Components.Entities;

public class MoodNote : ANote<MoodNote>, IMoodNoteAnalysis
{
    public DataAnalysis<MoodNote> MoodAnalysis { get; set; } = new();
    public FileService Mood { get; set; } = new();
    public MoodNote()
    {
        Db = FirestoreDb.Create(FirebaseApp.DefaultInstance.Options.ProjectId);
    }
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
    public override async Task LoadNoteLocalToCloud(string uid)
    {
        var lastId = NoteControl.Data.Last().Id;

        foreach (var item in NoteControl.Data.Select((value, index) => new { value, index }))
        {
            item.value.Id = (uint)item.index + lastId;
            await Db.Collection("Users").Document(uid).Collection("MoodNote").Document(item.value.Id.ToString()).SetAsync(item);
        }

        NoteControl.Data.Clear();
        await NoteControl.UpdateFile(Constants.PathMoodNotes);
    }
    public override async Task LoadNoteCloud(string uid)
    {
        var snapshot = await Db.Collection("Users").Document(uid).Collection("MoodNote").GetSnapshotAsync();

        NoteControl.Data.Append(snapshot.Documents.Select(doc =>
        {
            return new MoodNote
            {
                Id = doc.GetValue<uint>("id"),
                Date = doc.GetValue<DateTime>("date"),
                Text = doc.GetValue<string>("text"),
                Mood = new()
                {
                    Name = doc.GetValue<string>("name"),
                    Source = doc.GetValue<string>("source"),
                }
            };
        }).ToList());
    }
    public override async Task AddNote(MoodNote obj, string uid = null)
    {
        if (NoteControl.Data.Count > 0)
            obj.Id = NoteControl.Data.Last().Id + 1;

        if (uid != null)
        {
            await Db.Collection("Users").Document(uid).Collection("MoodNote").Document(obj.Id.ToString()).SetAsync(obj);
        }
        else
        {
            NoteControl.Add(obj);
            await NoteControl.UpdateFile(Constants.PathMoodNotes);
        }
    }
    public override async Task ChangeNote(MoodNote obj, string uid = null)
    {
        var index = NoteControl.Data.FindIndex(item => obj.Id == Id);
        if (index > -1)
        {
            if (uid != null)
            {
                await Db.Collection("Users").Document(uid).Collection("MoodNote").Document(obj.Id.ToString()).SetAsync(obj);
            }
            else
            {
                NoteControl.Change(index, obj);
                await NoteControl.UpdateFile(Constants.PathMoodNotes);
            }
        }
    }
    public override async Task DeleteNote(MoodNote obj, string uid = null)
    {
        var index = NoteControl.Data.FindIndex(item => obj.Id == Id);
        if (index > -1)
        {
            if (uid != null)
            {
                await Db.Collection("Users").Document(uid).Collection("MoodNote").Document(obj.Id.ToString()).DeleteAsync();
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