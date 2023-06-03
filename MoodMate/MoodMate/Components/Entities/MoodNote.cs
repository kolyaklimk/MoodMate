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

    public override async Task DeleteNoteLocal()
    {
        ClearNotes();
        await NoteControl.UpdateFile(Constants.PathMoodNotes);
    }

    public override async Task SaveLocalToCloud(Firebase.Auth.User user)
    {
        var data = GetData();
        while (data.Count != 0)
        {
            await NoteControl.AddAsync(user.Uid, "MoodNote", new Dictionary<string, object>
                {
                    { "Date", TimeZoneInfo.ConvertTimeToUtc(data[0].Date) },
                    { "Text", data[0].Text },
                    { "Name", data[0].Mood.Name },
                    { "Source", data[0].Mood.Source }
                });
            data.RemoveAt(0);
        }
        await NoteControl.UpdateFile(Constants.PathMoodNotes);
    }

    public override async Task LoadNoteCloud(int offset, int limit, Firebase.Auth.User user, bool refresh = true)
    {
        if (refresh)
            ClearNotes();

        var snapshot = await NoteControl.GetOrderByDateAsync(user.Uid, "MoodNote", offset, limit);

        foreach (var item in snapshot)
        {
            var dictionary = item.ToDictionary();

            NoteControl.Data.Add(new MoodNote
            {
                Id = item.Id,
                Date = ((Timestamp)dictionary["Date"]).ToDateTime().ToLocalTime(),
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
        if (user != null)
        {
            var rezult = await NoteControl.AddAsync(user.Uid, "MoodNote",
                new Dictionary<string, object>
                {
                    { "Date", TimeZoneInfo.ConvertTimeToUtc(obj.Date) },
                    { "Text", obj.Text },
                    { "Name", obj.Mood.Name },
                    { "Source", obj.Mood.Source }
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
                await NoteControl.UpdateAsync(user.Uid, "MoodNote", obj.Id.ToString(),
                    new Dictionary<string, object>
                    {
                        { "Date", TimeZoneInfo.ConvertTimeToUtc(obj.Date) },
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
                await NoteControl.DeleteAsync(user.Uid, "MoodNote", obj.Id);
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
    public async Task InitAnalyse(DateTime date, Firebase.Auth.User user = null)
    {
        MoodAnalysis.AnalysedData.Clear();

        if (user != null)
        {
            var firstDay = TimeZoneInfo.ConvertTimeToUtc(date);
            var lastDay = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).AddDays(1));

            var snapshot = await NoteControl.GetintervalAsync(user.Uid, "MoodNote", firstDay, lastDay);

            foreach (var item in snapshot)
            {
                var dictionary = item.ToDictionary();
                MoodAnalysis.AddItem(
                    dictionary["Name"]?.ToString(),
                    dictionary["Source"]?.ToString(),
                    ((Timestamp)dictionary["Date"]).ToDateTime().ToLocalTime(), date);
            }
        }
        else
        {
            foreach (var item in NoteControl.Data)
            {
                MoodAnalysis.AddItem(item.Mood.Name, item.Mood.Source, item.Date, date);
            }
        }
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